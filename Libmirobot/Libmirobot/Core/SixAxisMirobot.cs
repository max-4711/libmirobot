using Libmirobot.GCode;
using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Libmirobot.IO;
using System;
using System.Collections.Generic;
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
        private readonly bool delayInstructionUntilPreviousInstructionCompleted;

        private readonly SixAxisRobotSetupParameters setupParameters;

        private const int TIMER_TICKS_TO_UPDATE_STATUS_REQUEST = 6;
        private Timer telegramSendTimer = new Timer(50); //Mirobot reportedly struggles with receiving instructions in a higher frequency than 20 Hz: http://discuz.wlkata.com/forum.php?mod=viewthread&tid=8&extra=page%3D1
        private Queue<RobotTelegram> outboundTelegramQueue = new Queue<RobotTelegram>();
        private int timerTicksSinceStatusUpdate = 0;
        private bool readyToSendNewInstructionTelegram = true;
        private bool noStatusTelegramResponsePending = true;

        private SixAxisMirobot(SixAxisRobotSetupParameters setupParameters, bool delayInstructionUntilPreviousInstructionCompleted)
        {
            this.setupParameters = setupParameters;
            this.delayInstructionUntilPreviousInstructionCompleted = delayInstructionUntilPreviousInstructionCompleted;

            this.telegramSendTimer.Elapsed += this.TelegramSendTimer_Elapsed;
            this.telegramSendTimer.AutoReset = true;
            this.telegramSendTimer.Start();
        }

        /// <inheritdoc/>
        public void HomeAxes(HomingMode homingMode)
        {
            var homingInstruction = homingMode == HomingMode.InSequence ? this.setupParameters.SequentialHomingInstruction : this.setupParameters.SimultaneousHomingInstruction;

            var instructionCode = homingInstruction.GenerateGCode(new EmptyInstructionParameter());

            this.QueueInstruction(instructionCode, homingInstruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void IncrementAxes(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed)
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

            this.QueueInstruction(instructionCode, axisIncrementInstruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void IncrementCartesian(float xCoordinateIncrement, float yCoordinateIncrement, float zCoordinateIncrement, float xRotationIncrement, float yRotationIncrement, float zRotationIncrement, int speed, MovementMode movementMode)
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

            this.QueueInstruction(instructionCode, cartesianIncrementInstruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void MoveAxesTo(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed)
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

            this.QueueInstruction(instructionCode, axisMoveInstruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void MoveToCartesian(float xCoordinate, float yCoordinate, float zCoordinate, float xRotation, float yRotation, float zRotation, int speed, MovementMode movementMode)
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

            this.QueueInstruction(instructionCode, cartesianMoveInstruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void SetAirPumpPower(int pwm)
        {
            var airPumpInstruction = this.setupParameters.SetAirPumpPwmInstruction;

            var instructionCode = airPumpInstruction.GenerateGCode(new IntegerInstructionParameter { Parameter = pwm });

            this.QueueInstruction(instructionCode, airPumpInstruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void SetAxesHardLimit(bool on)
        {
            var axesHardLimitInstruction = this.setupParameters.SetAxesHardLimitInstruction;

            var instructionCode = axesHardLimitInstruction.GenerateGCode(new BinaryInstructionParameter { OpenClose = on });

            this.QueueInstruction(instructionCode, axesHardLimitInstruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void SetAxesSoftLimit(bool on)
        {
            var axesSoftLimitInstruction = this.setupParameters.SetAxesSoftLimitInstruction;

            var instructionCode = axesSoftLimitInstruction.GenerateGCode(new BinaryInstructionParameter { OpenClose = on });

            this.QueueInstruction(instructionCode, axesSoftLimitInstruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void SetGripperAperture(int pwm)
        {
            var gripperApertureInstruction = this.setupParameters.SetGripperPwmInstruction;

            var instructionCode = gripperApertureInstruction.GenerateGCode(new IntegerInstructionParameter { Parameter = pwm });

            this.QueueInstruction(instructionCode, gripperApertureInstruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void UnlockAxes()
        {
            var instruction = this.setupParameters.UnlockAxesInstruction;
            var instructionCode = instruction.GenerateGCode(new EmptyInstructionParameter());

            this.QueueInstruction(instructionCode, instruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void UpdateCurrentPosition()
        {
            var instruction = this.setupParameters.RequestPositionInstruction;
            var instructionCode = instruction.GenerateGCode(new EmptyInstructionParameter());

            this.QueueInstruction(instructionCode, instruction.UniqueIdentifier);
        }

        /// <inheritdoc/>
        public void AttachConnection(ISerialConnection serialConnection)
        {
            serialConnection.TelegramReceived -= this.SerialConnection_TelegramReceived;
            serialConnection.TelegramReceived += this.SerialConnection_TelegramReceived;
        }

        private void TelegramSendTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.readyToSendNewInstructionTelegram == false)
            {
                if (this.noStatusTelegramResponsePending)
                {
                    this.timerTicksSinceStatusUpdate++;
                }                

                if (this.timerTicksSinceStatusUpdate > TIMER_TICKS_TO_UPDATE_STATUS_REQUEST)
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

            if (this.outboundTelegramQueue.Count > 0)
            {
                var telegram = this.outboundTelegramQueue.Dequeue();
                this.SendTelegram(telegram);
                if (this.delayInstructionUntilPreviousInstructionCompleted)
                {
                    var command = this.setupParameters.AllInstructions.FirstOrDefault(x => x.UniqueIdentifier == telegram.InstructionIdentifier);
                    if (command != null && command.IsMotionInstruction)
                    {
                        this.readyToSendNewInstructionTelegram = false;
                        this.timerTicksSinceStatusUpdate = 0;
                        this.noStatusTelegramResponsePending = true;
                    }
                }
            }
        }

        private void SerialConnection_TelegramReceived(object sender, RobotTelegram e)
        {
            var responsedInstruction = this.setupParameters.AllInstructions.FirstOrDefault(x => x.CanProcessResponse(e.Data));
            if (responsedInstruction == null)
                return;            

            var updatedRobotState = responsedInstruction.ProcessResponse(e.Data);
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

            this.SendRobotStatusUpdate(updatedRobotState);
        }

        private void QueueInstruction(string instruction, string instructionIdentifier)
        {
            this.outboundTelegramQueue.Enqueue(new RobotTelegram(instructionIdentifier, instruction));
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
        /// <returns>Newly instanciated six axis robot.</returns>
        public static SixAxisMirobot CreateNew(bool delayInstructionUntilPreviousInstructionCompleted = true)
        {
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
                ), delayInstructionUntilPreviousInstructionCompleted);
        }
    }
}
