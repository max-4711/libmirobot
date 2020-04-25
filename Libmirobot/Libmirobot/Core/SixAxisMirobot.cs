using Libmirobot.GCode;
using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Libmirobot.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Timers;

namespace Libmirobot.Core
{
    /// <summary>
    /// Represents the implementation of a 6 (+1) axis robot.
    /// </summary>
    public class SixAxisMirobot : ISixAxisRobot
    {
        /// <inheritdoc/>
        public event EventHandler<RobotTelegram>? InstructionSent;
        /// <inheritdoc/>
        public event EventHandler<RobotStateChangedEventArgs>? RobotStateChanged;
        /// <inheritdoc/>
        public event EventHandler<RobotErrorEventArgs>? RobotErrorOccurred;
        /// <inheritdoc/>
        public event EventHandler<RobotResetEventArgs>? RobotResetOccurred;

        private readonly bool delayInstructionUntilPreviousInstructionCompleted;
        private readonly bool autoHomeAxes;

        private readonly SixAxisRobotSetupParameters setupParameters;

        private readonly int timerTicksToUpdateStatusRequest;
        private Timer telegramSendTimer = new Timer(50); //Mirobot reportedly struggles with receiving instructions in a higher frequency than 20 Hz: http://discuz.wlkata.com/forum.php?mod=viewthread&tid=8&extra=page%3D1
        private Queue<RobotTelegram> outboundTelegramQueue = new Queue<RobotTelegram>();
        private int timerTicksSinceStatusUpdate = 0;
        private bool readyToSendNewInstructionTelegram = true;
        private bool noStatusTelegramResponsePending = true;
        private bool homingNeeded = false;

        private object statusFlagsLockObject = new object();

        private RobotStatusUpdate? lastRobotStatusUpdate = null;
        private RobotPositionParameter? angleModeTargetPosition = null;
        private RobotPositionParameter? cartesianModeTargetPosition = null;
        private RobotTelegram? lastSentMotionInstruction = null;

        private SixAxisMirobot(SixAxisRobotSetupParameters setupParameters, bool delayInstructionUntilPreviousInstructionCompleted, int timerTicksToUpdateStatusRequest, bool autoHomeAxes)
        {
            this.setupParameters = setupParameters;
            this.delayInstructionUntilPreviousInstructionCompleted = delayInstructionUntilPreviousInstructionCompleted;
            this.timerTicksToUpdateStatusRequest = timerTicksToUpdateStatusRequest;
            this.autoHomeAxes = autoHomeAxes;

            this.telegramSendTimer.Elapsed += this.TelegramSendTimer_Elapsed;
            this.telegramSendTimer.AutoReset = true;
            this.telegramSendTimer.Start();
        }

        /// <inheritdoc/>
        public void HomeAxes(HomingMode homingMode = HomingMode.Simultaneous)
        {
            var homingInstruction = homingMode == HomingMode.InSequence ? this.setupParameters.SequentialHomingInstruction : this.setupParameters.SimultaneousHomingInstruction;

            var instructionCode = homingInstruction.GenerateGCode(new EmptyInstructionParameter());

            Func<RobotPositionParameter?, RobotPositionParameter?> positionModificator = (RobotPositionParameter? oldTargetPosition) =>
            {
                return null;
            };

            this.QueueInstruction(instructionCode, homingInstruction.UniqueIdentifier, positionModificator, positionModificator);
        }

        /// <inheritdoc/>
        public void IncrementAxes(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed = 2000)
        {
            var axisIncrementInstruction = this.setupParameters.AngleRelativeMoveInstruction;

            var instructionCode = axisIncrementInstruction.GenerateGCode(new MotionInstructionParameter 
            {
                PositioningParameter1 = axis1,
                PositioningParameter2 = axis2,
                PositioningParameter3 = axis3,
                PositioningParameter4 = axis4,
                PositioningParameter5 = axis5,
                PositioningParameter6 = axis6,
                Speed = speed
            });

            Func<RobotPositionParameter?, RobotPositionParameter?> angleTargetPositionModificator = (RobotPositionParameter? oldTargetPosition) =>
            {
                if (oldTargetPosition == null)
                    return null;

                return new RobotPositionParameter
                {
                    PositioningParameter1 = oldTargetPosition.PositioningParameter1 + axis1,
                    PositioningParameter2 = oldTargetPosition.PositioningParameter2 + axis2,
                    PositioningParameter3 = oldTargetPosition.PositioningParameter3 + axis3,
                    PositioningParameter4 = oldTargetPosition.PositioningParameter4 + axis4,
                    PositioningParameter5 = oldTargetPosition.PositioningParameter5 + axis5,
                    PositioningParameter6 = oldTargetPosition.PositioningParameter6 + axis6
                };
            };

            Func<RobotPositionParameter?, RobotPositionParameter?> cartesianTargetPositionModificator = (RobotPositionParameter? oldTargetPosition) =>
            {
                return null;
            };

            this.QueueInstruction(instructionCode, axisIncrementInstruction.UniqueIdentifier, angleTargetPositionModificator, cartesianTargetPositionModificator);
        }

        /// <inheritdoc/>
        public void IncrementCartesian(float xCoordinateIncrement, float yCoordinateIncrement, float zCoordinateIncrement, float xRotationIncrement, float yRotationIncrement, float zRotationIncrement, int speed = 2000, MovementMode movementMode = MovementMode.Linear)
        {
            var cartesianIncrementInstruction = movementMode == MovementMode.Linear ? this.setupParameters.CartesianRelativeLinMoveInstruction : this.setupParameters.CartesianRelativePtpMoveInstruction;

            var instructionCode = cartesianIncrementInstruction.GenerateGCode(new MotionInstructionParameter
            {
                PositioningParameter1 = xCoordinateIncrement,
                PositioningParameter2 = yCoordinateIncrement,
                PositioningParameter3 = zCoordinateIncrement,
                PositioningParameter4 = xRotationIncrement,
                PositioningParameter5 = yRotationIncrement,
                PositioningParameter6 = zRotationIncrement,
                Speed = speed
            });

            Func<RobotPositionParameter?, RobotPositionParameter?> cartesianTargetPositionModificator = (RobotPositionParameter? oldTargetPosition) =>
            {
                if (oldTargetPosition == null)
                    return null;

                return new RobotPositionParameter
                {
                    PositioningParameter1 = oldTargetPosition.PositioningParameter1 + xCoordinateIncrement,
                    PositioningParameter2 = oldTargetPosition.PositioningParameter2 + yCoordinateIncrement,
                    PositioningParameter3 = oldTargetPosition.PositioningParameter3 + zCoordinateIncrement,
                    PositioningParameter4 = oldTargetPosition.PositioningParameter4 + xRotationIncrement,
                    PositioningParameter5 = oldTargetPosition.PositioningParameter5 + yRotationIncrement,
                    PositioningParameter6 = oldTargetPosition.PositioningParameter6 + zRotationIncrement
                };
            };

            Func<RobotPositionParameter?, RobotPositionParameter?> angleTargetPositionModificator = (RobotPositionParameter? oldTargetPosition) =>
            {
                return null;
            };

            this.QueueInstruction(instructionCode, cartesianIncrementInstruction.UniqueIdentifier, angleTargetPositionModificator, cartesianTargetPositionModificator);
        }

        /// <inheritdoc/>
        public void MoveAxesTo(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed = 2000)
        {
            var axisMoveInstruction = this.setupParameters.AngleAbsoluteMoveInstrcution;

            var instructionCode = axisMoveInstruction.GenerateGCode(new MotionInstructionParameter
            {
                PositioningParameter1 = axis1,
                PositioningParameter2 = axis2,
                PositioningParameter3 = axis3,
                PositioningParameter4 = axis4,
                PositioningParameter5 = axis5,
                PositioningParameter6 = axis6,
                Speed = speed
            });

            Func<RobotPositionParameter?, RobotPositionParameter?> angleTargetPositionModificator = (RobotPositionParameter? oldTargetPosition) =>
            {
                return new RobotPositionParameter
                {
                    PositioningParameter1 = axis1,
                    PositioningParameter2 = axis2,
                    PositioningParameter3 = axis3,
                    PositioningParameter4 = axis4,
                    PositioningParameter5 = axis5,
                    PositioningParameter6 = axis6
                };
            };

            Func<RobotPositionParameter?, RobotPositionParameter?> cartesianTargetPositionModificator = (RobotPositionParameter? oldTargetPosition) =>
            {
                return null;
            };

            this.QueueInstruction(instructionCode, axisMoveInstruction.UniqueIdentifier, angleTargetPositionModificator, cartesianTargetPositionModificator);
        }

        /// <inheritdoc/>
        public void MoveToCartesian(float xCoordinate, float yCoordinate, float zCoordinate, float xRotation, float yRotation, float zRotation, int speed = 2000, MovementMode movementMode = MovementMode.Linear)
        {
            var cartesianMoveInstruction = movementMode == MovementMode.Linear ? this.setupParameters.CartesianAbsoluteLinMoveInstruction : this.setupParameters.CartesianAbsolutePtpMoveInstrcution;

            var instructionCode = cartesianMoveInstruction.GenerateGCode(new MotionInstructionParameter
            {
                PositioningParameter1 = xCoordinate,
                PositioningParameter2 = yCoordinate,
                PositioningParameter3 = zCoordinate,
                PositioningParameter4 = xRotation,
                PositioningParameter5 = yRotation,
                PositioningParameter6 = zRotation,
                Speed = speed
            });

            Func<RobotPositionParameter?, RobotPositionParameter?> cartesianTargetPositionModificator = (RobotPositionParameter? oldTargetPosition) =>
            {
                return new RobotPositionParameter
                {
                    PositioningParameter1 = xCoordinate,
                    PositioningParameter2 = yCoordinate,
                    PositioningParameter3 = zCoordinate,
                    PositioningParameter4 = xRotation,
                    PositioningParameter5 = yRotation,
                    PositioningParameter6 = zRotation
                };
            };

            Func<RobotPositionParameter?, RobotPositionParameter?> angleTargetPositionModificator = (RobotPositionParameter? oldTargetPosition) =>
            {
                return null;
            };

            this.QueueInstruction(instructionCode, cartesianMoveInstruction.UniqueIdentifier, angleTargetPositionModificator, cartesianTargetPositionModificator);
        }

        /// <inheritdoc/>
        public void SetAirPumpPower(int pwm)
        {
            var airPumpInstruction = this.setupParameters.SetAirPumpPwmInstruction;

            var instructionCode = airPumpInstruction.GenerateGCode(new IntegerInstructionParameter { Parameter = pwm });

            this.QueueInstruction(instructionCode, airPumpInstruction.UniqueIdentifier, null, null);
        }

        /// <inheritdoc/>
        public void SetAxesHardLimit(bool on)
        {
            var axesHardLimitInstruction = this.setupParameters.SetAxesHardLimitInstruction;

            var instructionCode = axesHardLimitInstruction.GenerateGCode(new BinaryInstructionParameter { OpenClose = on });

            this.QueueInstruction(instructionCode, axesHardLimitInstruction.UniqueIdentifier, null, null);
        }

        /// <inheritdoc/>
        public void SetAxesSoftLimit(bool on)
        {
            var axesSoftLimitInstruction = this.setupParameters.SetAxesSoftLimitInstruction;

            var instructionCode = axesSoftLimitInstruction.GenerateGCode(new BinaryInstructionParameter { OpenClose = on });

            this.QueueInstruction(instructionCode, axesSoftLimitInstruction.UniqueIdentifier, null, null);
        }

        /// <inheritdoc/>
        public void SetGripperAperture(int pwm)
        {
            var gripperApertureInstruction = this.setupParameters.SetGripperPwmInstruction;

            var instructionCode = gripperApertureInstruction.GenerateGCode(new IntegerInstructionParameter { Parameter = pwm });

            this.QueueInstruction(instructionCode, gripperApertureInstruction.UniqueIdentifier, null, null);
        }

        /// <inheritdoc/>
        public void UnlockAxes()
        {
            var instruction = this.setupParameters.UnlockAxesInstruction;
            var instructionCode = instruction.GenerateGCode(new EmptyInstructionParameter());

            this.QueueInstruction(instructionCode, instruction.UniqueIdentifier, null, null);
        }

        /// <inheritdoc/>
        public void UpdateCurrentPosition()
        {
            var instruction = this.setupParameters.RequestPositionInstruction;
            var instructionCode = instruction.GenerateGCode(new EmptyInstructionParameter());

            this.QueueInstruction(instructionCode, instruction.UniqueIdentifier, null, null);
        }

        /// <inheritdoc/>
        public void AttachConnection(ISerialConnection serialConnection)
        {
            serialConnection.TelegramReceived -= this.SerialConnection_TelegramReceived;
            serialConnection.TelegramReceived += this.SerialConnection_TelegramReceived;
        }

        /// <inheritdoc/>
        public void ClearCommandQueue()
        {
            lock (this.outboundTelegramQueue)
            {
                this.outboundTelegramQueue.Clear();
            }
        }

        /// <inheritdoc/>
        public void Reset()
        {
            lock (this.statusFlagsLockObject)
            {
                this.timerTicksSinceStatusUpdate = 0;
                this.readyToSendNewInstructionTelegram = true;
                this.noStatusTelegramResponsePending = true;
                this.homingNeeded = false;
                this.lastRobotStatusUpdate = null;
            }
        }

        private void TelegramSendTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.statusFlagsLockObject)
            {
                //Need to request status first?
                if (this.readyToSendNewInstructionTelegram == false)
                {
                    if (this.noStatusTelegramResponsePending)
                    {
                        this.timerTicksSinceStatusUpdate++;
                    }

                    if (this.timerTicksSinceStatusUpdate > this.timerTicksToUpdateStatusRequest)
                    {
                        this.timerTicksSinceStatusUpdate = 0;
                        this.noStatusTelegramResponsePending = false;

                        var instruction = this.setupParameters.RequestPositionInstruction;
                        var instructionCode = instruction.GenerateGCode(new EmptyInstructionParameter());
                        var telegram = new RobotTelegram(instruction.UniqueIdentifier, instructionCode);
                        this.SendTelegram(telegram);
                    }

                    return;
                }

                //Need to perform homing, because reset has occured or homing wasn't performed yet at all?
                if (this.autoHomeAxes && this.homingNeeded)
                {
                    var homingInstruction = this.setupParameters.SimultaneousHomingInstruction;
                    var homingGCode = homingInstruction.GenerateGCode(new EmptyInstructionParameter());
                    var homingTelegram = new RobotTelegram(homingInstruction.UniqueIdentifier, homingGCode);
                    this.SendTelegram(homingTelegram);
                    this.homingNeeded = false;

                    if (this.delayInstructionUntilPreviousInstructionCompleted)
                    {
                        this.readyToSendNewInstructionTelegram = false;
                        this.timerTicksSinceStatusUpdate = 0;
                        this.noStatusTelegramResponsePending = true;
                    }

                    return;
                }

                //Need to re-send previous motion instruction because execution failed? (bound to auto-home-setting, because motion->error->homing->motion can be seen as part of auto-home-function)
                if (this.autoHomeAxes && this.lastRobotStatusUpdate != null && this.lastRobotStatusUpdate.IsIdle == true && this.lastSentMotionInstruction != null)
                {
                    if (!this.RobotIsAtTargetPosition())
                    {
                        this.SendTelegram(this.lastSentMotionInstruction);
                        if (this.delayInstructionUntilPreviousInstructionCompleted)
                        {
                            this.readyToSendNewInstructionTelegram = false;
                            this.timerTicksSinceStatusUpdate = 0;
                            this.noStatusTelegramResponsePending = true;
                        }
                        return;
                    }
                    this.lastSentMotionInstruction = null; //This line has no impact on functionality, but will prevent the RobotIsAtTargetPosition from being executed in the timer loop
                }

                //Finally: Ready to send next 'regular' instruction
                var queuedTelegram = this.TryGetQueuedTelegram();
                if (queuedTelegram != null)
                {
                    var instructionIsMotionInstruction = this.setupParameters.AllInstructions.Any(x => x.UniqueIdentifier == queuedTelegram.InstructionIdentifier && x.IsMotionInstruction);
                    if (instructionIsMotionInstruction)
                    {
                        this.lastSentMotionInstruction = queuedTelegram;
                        if (queuedTelegram.RobotAngleTargetPositionModificator != null)
                        {
                            this.angleModeTargetPosition = queuedTelegram.RobotAngleTargetPositionModificator(RobotPositionParameter.FromRobotStatusUpdateAngle(this.lastRobotStatusUpdate));
                        }
                        if (queuedTelegram.RobotCartesianTargetPositionModificator != null)
                        {
                            this.cartesianModeTargetPosition = queuedTelegram.RobotCartesianTargetPositionModificator(RobotPositionParameter.FromRobotStatusUpdateCartesian(this.lastRobotStatusUpdate));
                        }
                    }
                    else
                    {
                        this.lastSentMotionInstruction = null;
                    }

                    this.SendTelegram(queuedTelegram);
                    if (this.delayInstructionUntilPreviousInstructionCompleted)
                    {
                        if (instructionIsMotionInstruction)
                        {
                            this.readyToSendNewInstructionTelegram = false;
                            this.timerTicksSinceStatusUpdate = 0;
                            this.noStatusTelegramResponsePending = true;
                        }
                    }
                }
                
            }
        }

        private bool RobotIsAtTargetPosition()
        {
            if (this.lastRobotStatusUpdate == null)
                return true;

            if (this.angleModeTargetPosition == null && this.cartesianModeTargetPosition == null)
                return true;

            if (this.angleModeTargetPosition != null)
            {
                var currentAnglePosition = RobotPositionParameter.FromRobotStatusUpdateAngle(this.lastRobotStatusUpdate);
                if (currentAnglePosition == this.angleModeTargetPosition)
                    return true;
            }

            if (this.cartesianModeTargetPosition != null)
            {
                var currentCartesianPosition = RobotPositionParameter.FromRobotStatusUpdateCartesian(this.lastRobotStatusUpdate);
                if (currentCartesianPosition == this.cartesianModeTargetPosition)
                    return true;
            }

            return false;
        }

        private RobotTelegram? TryGetQueuedTelegram() //Written manually, because TryDequeue does not exist in .NET Standard 2.0
        {
            lock (this.outboundTelegramQueue)
            {
                if (this.outboundTelegramQueue.Count > 0)
                {
                    return this.outboundTelegramQueue.Dequeue();
                }
                else
                {
                    return null;
                }
            }
        }

        private void SerialConnection_TelegramReceived(object sender, RobotTelegram e)
        {
            if (e.Data.Contains("ALARM"))
            {
                this.RobotErrorOccurred?.Invoke(this, new RobotErrorEventArgs(e.Data));
                lock (this.statusFlagsLockObject)
                {
                    this.angleModeTargetPosition = null;
                    this.cartesianModeTargetPosition = null;
                }                
            }
            else if (e.Data.Contains("Qinnew Robot") || e.Data.Contains(" based on Grbl "))
            {
                this.RobotResetOccurred?.Invoke(this, new RobotResetEventArgs());
                lock (this.statusFlagsLockObject)
                {
                    this.homingNeeded = true;

                    this.noStatusTelegramResponsePending = true;
                    this.timerTicksSinceStatusUpdate = 0;
                    //Optional: Add 'this.readyToSendNewInstructionTelegram = true;' here to skip status request after reboot
                }
            }
            else if (e.Data.Contains("Locked status"))
            {
                lock (this.statusFlagsLockObject)
                {
                    this.homingNeeded = true;
                }
            }
            else if (e.Data.Contains("Soft limit") || e.Data.Contains("Hard limit"))
            {
                this.RobotErrorOccurred?.Invoke(this, new RobotErrorEventArgs(e.Data));
                lock (this.statusFlagsLockObject)
                {
                    this.angleModeTargetPosition = null;
                    this.cartesianModeTargetPosition = null;
                }                
            }

            var responsedInstruction = this.setupParameters.AllInstructions.FirstOrDefault(x => x.UniqueIdentifier == e.InstructionIdentifier && x.CanProcessResponse(e.Data));
            if (responsedInstruction == null)
            {
                responsedInstruction = this.setupParameters.AllInstructions.FirstOrDefault(x => x.CanProcessResponse(e.Data));
                if (responsedInstruction == null)
                    return;
            }

            var updatedRobotState = responsedInstruction.ProcessResponse(e.Data);

            lock (this.statusFlagsLockObject)
            {
                if (this.delayInstructionUntilPreviousInstructionCompleted)
                {
                    if (updatedRobotState.HasData)
                    {
                        this.noStatusTelegramResponsePending = true;
                    }

                    if (updatedRobotState.IsIdle == true)
                    {
                        this.readyToSendNewInstructionTelegram = true;
                    }
                }

                if (updatedRobotState.HasData)
                    this.lastRobotStatusUpdate = updatedRobotState;
            }

            this.SendRobotStatusUpdate(updatedRobotState);
        }

        private void QueueInstruction(string instruction, string instructionIdentifier, Func<RobotPositionParameter?, RobotPositionParameter?>? robotAngleTargetPositionModificator, Func<RobotPositionParameter?, RobotPositionParameter?>? robotCartesianTargetPositionModificator)
        {
            var telegram = new RobotTelegram(instructionIdentifier, instruction);
            telegram.RobotAngleTargetPositionModificator = robotAngleTargetPositionModificator;
            telegram.RobotCartesianTargetPositionModificator = robotCartesianTargetPositionModificator;

            lock (this.outboundTelegramQueue)
            {
                this.outboundTelegramQueue.Enqueue(telegram);
            }            
        }

        private void SendTelegram(RobotTelegram robotTelegram)
        {
            this.InstructionSent?.Invoke(this, robotTelegram);
        }

        private void SendRobotStatusUpdate(RobotStatusUpdate robotStatusUpdate)
        {
            if (!robotStatusUpdate.HasData)
                return;            

            this.RobotStateChanged?.Invoke(this, new RobotStateChangedEventArgs 
            {
                IsIdle = robotStatusUpdate.IsIdle ?? true,
                Axis1Angle = robotStatusUpdate.Axis1Angle ?? 0,
                Axis2Angle = robotStatusUpdate.Axis2Angle ?? 0,
                Axis3Angle = robotStatusUpdate.Axis3Angle ?? 0,
                Axis4Angle = robotStatusUpdate.Axis4Angle ?? 0,
                Axis5Angle = robotStatusUpdate.Axis5Angle ?? 0,
                Axis6Angle = robotStatusUpdate.Axis6Angle ?? 0,
                ExternalSlideRail = robotStatusUpdate.ExternalSlideRail ?? 0,
                GripperPwm = robotStatusUpdate.Pwm2 ?? 0,
                PneumaticPumpPwm = robotStatusUpdate.Pwm1 ?? 0,
                XCoordinate = robotStatusUpdate.XCoordinate ?? 0,
                YCoordinate = robotStatusUpdate.YCoordinate ?? 0,
                ZCoordinate = robotStatusUpdate.ZCoordinate ?? 0,
                XRotation = robotStatusUpdate.XRotation ?? 0,
                YRotation = robotStatusUpdate.YRotation ?? 0,
                ZRotation = robotStatusUpdate.ZRotation ?? 0
            });
        }

        /// <summary>
        /// Frees resources allocated by this robot instance and will stop sending telegrams.
        /// </summary>
        public void Dispose()
        {
            this.telegramSendTimer.Stop();
            this.telegramSendTimer.Dispose();
        }

        /// <summary>
        /// Created a new instance of six axis mirobot with default configuration.
        /// </summary>
        /// <param name="autoHomeAxes">If true, a homing operation will automatically be executed if needed. Only available, if 'delayInstructionUntilPreviousInstructionCompleted' is true.</param>
        /// <param name="delayInstructionUntilPreviousInstructionCompleted">If true, motion instructions will be sent to the hardware one after another, only if the previous instruction is completed.</param>
        /// <param name="timerTicksToUpdateStatusRequest">Specifies, how often motion instruction completion is being polled from the hardware if 'delayInstructionUntilPreviousInstructionCompleted' is true (value will be multiplicated with 50 ms)</param>
        /// <returns>Newly instanciated six axis robot.</returns>
        public static SixAxisMirobot CreateNew(bool delayInstructionUntilPreviousInstructionCompleted = true, int timerTicksToUpdateStatusRequest = 7, bool autoHomeAxes = true)
        {
            if (delayInstructionUntilPreviousInstructionCompleted == false)
                autoHomeAxes = false;

            return new SixAxisMirobot(new SixAxisRobotSetupParameters(
                new ObtainStatusInstruction(),
                new CartesianModeAbsolutePtpMotionInstruction(),
                new CartesianModeAbsoluteLinMotionInstruction(),
                new CartesianModeRelativePtpMotionInstruction(),
                new CartesianModeRelativeLinMotionInstruction(),
                new AngleModeAbsoluteMotionInstruction(),
                new AngleModeIncrementalMotionInstruction(),
                new ToggleAxesSoftLimitInstruction(),
                new ToggleAxesHardLimitInstruction(),
                new UnlockAxesInstruction(),
                new HomingSequentialInstruction(),
                new HomingSimultaneousInstruction(),
                new SwitchAirPumpInstruction(),
                new SwitchGripperInstruction()
                ), delayInstructionUntilPreviousInstructionCompleted, timerTicksToUpdateStatusRequest, autoHomeAxes);
        }
    }
}
