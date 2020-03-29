using Libmirobot.GCode;
using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Libmirobot.IO;
using System;
using System.Linq;

namespace Libmirobot.Core
{
    public class SixAxisMirobot : ISixAxisRobot
    {
        public event EventHandler<RobotTelegram> InstructionSent;
        public event EventHandler<RobotStateChangedEventArgs> RobotStateChanged;

        public bool AutoSendStatusUpdateRequests { get; set; } = false;

        private readonly SixAxisRobotSetupParameters setupParameters;

        private SixAxisMirobot(SixAxisRobotSetupParameters setupParameters)
        {
            this.setupParameters = setupParameters;
        }

        public void HomeAxes(HomingMode homingMode)
        {
            var homingInstruction = homingMode == HomingMode.InSequence ? this.setupParameters.SequentialHomingInstruction : this.setupParameters.SimultaneousHomingInstruction;

            var instructionCode = homingInstruction.GenerateGCode(new EmptyInstructionParameter());

            this.SendInstruction(instructionCode, homingInstruction.UniqueIdentifier);

            if (this.AutoSendStatusUpdateRequests)
                this.UpdateCurrentPosition();
        }

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

            this.SendInstruction(instructionCode, axisIncrementInstruction.UniqueIdentifier);

            if (this.AutoSendStatusUpdateRequests)
                this.UpdateCurrentPosition();
        }

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

            this.SendInstruction(instructionCode, cartesianIncrementInstruction.UniqueIdentifier);

            if (this.AutoSendStatusUpdateRequests)
                this.UpdateCurrentPosition();
        }

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

            this.SendInstruction(instructionCode, axisMoveInstruction.UniqueIdentifier);

            if (this.AutoSendStatusUpdateRequests)
                this.UpdateCurrentPosition();
        }

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

            this.SendInstruction(instructionCode, cartesianMoveInstruction.UniqueIdentifier);

            if (this.AutoSendStatusUpdateRequests)
                this.UpdateCurrentPosition();
        }

        public void SetAirPumpPower(int pwm)
        {
            var airPumpInstruction = this.setupParameters.SetAirPumpPwmInstruction;

            var instructionCode = airPumpInstruction.GenerateGCode(new IntegerInstructionParameter { Parameter = pwm });

            this.SendInstruction(instructionCode, airPumpInstruction.UniqueIdentifier);

            if (this.AutoSendStatusUpdateRequests)
                this.UpdateCurrentPosition();
        }

        public void SetAxesHardLimit(bool on)
        {
            var axesHardLimitInstruction = this.setupParameters.SetAxesHardLimitInstruction;

            var instructionCode = axesHardLimitInstruction.GenerateGCode(new BinaryInstructionParameter { OpenClose = on });

            this.SendInstruction(instructionCode, axesHardLimitInstruction.UniqueIdentifier);
        }

        public void SetAxesSoftLimit(bool on)
        {
            var axesSoftLimitInstruction = this.setupParameters.SetAxesSoftLimitInstruction;

            var instructionCode = axesSoftLimitInstruction.GenerateGCode(new BinaryInstructionParameter { OpenClose = on });

            this.SendInstruction(instructionCode, axesSoftLimitInstruction.UniqueIdentifier);
        }

        public void SetGripperAperture(int pwm)
        {
            var gripperApertureInstruction = this.setupParameters.SetGripperPwmInstruction;

            var instructionCode = gripperApertureInstruction.GenerateGCode(new IntegerInstructionParameter { Parameter = pwm });

            this.SendInstruction(instructionCode, gripperApertureInstruction.UniqueIdentifier);

            if (this.AutoSendStatusUpdateRequests)
                this.UpdateCurrentPosition();
        }

        public void UnlockAxes()
        {
            var instruction = this.setupParameters.UnlockAxesInstruction;
            var instructionCode = instruction.GenerateGCode(new EmptyInstructionParameter());

            this.SendInstruction(instructionCode, instruction.UniqueIdentifier);
        }

        public void UpdateCurrentPosition()
        {
            var instruction = this.setupParameters.RequestPositionInstruction;
            var instructionCode = instruction.GenerateGCode(new EmptyInstructionParameter());

            this.SendInstruction(instructionCode, instruction.UniqueIdentifier);
        }

        public static SixAxisMirobot CreateNew()
        {
            return new SixAxisMirobot(new SixAxisRobotSetupParameters
            {
                AngleAbsoluteMoveInstrcution = new AngleModeAbsoluteMotionInstruction(),
                AngleRelativeMoveInstruction = new AngleModeIncrementalMotionInstruction(),
                CartesianAbsoluteLinMoveInstruction = new CartesianModeAbsoluteLinMotionInstruction(),
                CartesianAbsolutePtpMoveInstrcution = new CartesianModeAbsolutePtpMotionInstruction(),
                CartesianRelativeLinMoveInstruction = new CartesianModeRelativeLinMotionInstruction(),
                CartesianRelativePtpMoveInstruction = new CartesianModeRelativePtpMotionInstruction(),
                RequestPositionInstruction = new ObtainStatusInstruction(),
                SequentialHomingInstruction = new HomingSequentialInstruction(),
                SimultaneousHomingInstruction = new HomingSimultaneousInstruction(),
                SetAirPumpPwmInstruction = new SwitchAirPumpInstruction(),
                SetGripperPwmInstruction = new SwitchGripperInstruction(),
                SetAxesHardLimitInstruction = new ToggleAxesHardLimitInstruction(),
                SetAxesSoftLimitInstruction = new ToggleAxesSoftLimitInstruction(),
                UnlockAxesInstruction = new UnlockAxesInstruction()
            });
        }

        public void AttachConnection(ISerialConnection serialConnection)
        {
            serialConnection.TelegramReceived -= SerialConnection_TelegramReceived;
            serialConnection.TelegramReceived += SerialConnection_TelegramReceived;
        }

        private void SerialConnection_TelegramReceived(object sender, RobotTelegram e)
        {
            var responsedInstruction = this.setupParameters.AllInstructions.FirstOrDefault(x => x.CanProcessResponse(e.Data));
            if (responsedInstruction == null)
                return;

            var updatedRobotState = responsedInstruction.ProcessResponse(e.Data);

            this.ProcessRobotStateUpdate(updatedRobotState);
        }

        private void SendInstruction(string instruction, string instructionIdentifier)
        {
            this.InstructionSent?.Invoke(this, new RobotTelegram { Data = instruction, InstructionIdentifier = instructionIdentifier });
        }

        private void ProcessRobotStateUpdate(RobotStatusUpdate robotStatusUpdate)
        {
            if (!robotStatusUpdate.HasData)
                return;

            this.RobotStateChanged?.Invoke(this, new RobotStateChangedEventArgs 
            {
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
    }
}
