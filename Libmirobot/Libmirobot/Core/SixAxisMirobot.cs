using Libmirobot.GCode;
using Libmirobot.GCode.InstructionParameters;
using Libmirobot.IO;
using System;

namespace Libmirobot.Core
{
    public class SixAxisMirobot : ISixAxisRobot
    {
        private readonly SixAxisRobotSetupParameters setupParameters;

        public event EventHandler<InstructionSentEventArgs> InstructionSent;

        private SixAxisMirobot(SixAxisRobotSetupParameters setupParameters)
        {
            this.setupParameters = setupParameters;
        }

        public void HomeAxes(HomingMode homingMode)
        {
            var homingInstruction = homingMode == HomingMode.InSequence ? this.setupParameters.SequentialHomingInstruction : this.setupParameters.SimultaneousHomingInstruction;

            var instructionCode = homingInstruction.GenerateGCode(new EmptyInstructionParameter());

            this.SendInstruction(instructionCode, homingInstruction.UniqueIdentifier);
        }

        public void IncrementAxes(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed, MovementMode movementMode)
        {
            var axisIncrementInstruction = movementMode == MovementMode.Linear ? this.setupParameters.AngleRelativeLinMoveInstruction : this.setupParameters.AngleRelativePtpMoveInstruction;

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
        }

        public void MoveAxesTo(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed, MovementMode movementMode)
        {
            var axisMoveInstruction = movementMode == MovementMode.Linear ? this.setupParameters.AngleAbsoluteLinMoveInstruction : this.setupParameters.AngleAbsolutePtpMoveInstrcution;

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
        }

        public void SetAirPumpPower(int pwm)
        {
            var airPumpInstruction = this.setupParameters.SetAirPumpPwmInstruction;

            var instructionCode = airPumpInstruction.GenerateGCode(new IntegerInstructionParameter { Parameter = pwm });

            this.SendInstruction(instructionCode, airPumpInstruction.UniqueIdentifier);
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
            });
        }

        public void AttachToSerialConnection(ISerialConnection serialConnection)
        {
            serialConnection.MessageReceived -= SerialConnection_MessageReceived;
            serialConnection.MessageReceived += SerialConnection_MessageReceived;
        }

        private void SerialConnection_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SendInstruction(string instruction, string instructionIdentifier)
        {
            this.InstructionSent?.Invoke(this, new InstructionSentEventArgs { Instruction = instruction, InstructionIdentifier = instructionIdentifier });
        }
    }
}
