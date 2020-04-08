using Libmirobot.GCode;
using Libmirobot.GCode.InstructionParameters;
using System.Collections.Generic;

namespace Libmirobot.Core
{
    internal class SixAxisRobotSetupParameters
    {
        public SixAxisRobotSetupParameters(IGCodeInstruction<EmptyInstructionParameter> requestPositionInstruction,
            IGCodeInstruction<MotionInstructionParameter> cartesianAbsolutePtpMoveInstrcution,
            IGCodeInstruction<MotionInstructionParameter> cartesianAbsoluteLinMoveInstruction,
            IGCodeInstruction<MotionInstructionParameter> cartesianRelativePtpMoveInstruction,
            IGCodeInstruction<MotionInstructionParameter> cartesianRelativeLinMoveInstruction,
            IGCodeInstruction<MotionInstructionParameter> angleAbsoluteMoveInstruction,
            IGCodeInstruction<MotionInstructionParameter> angleRelativeMoveInstruction,
            IGCodeInstruction<BinaryInstructionParameter> setAxesSoftLimitInstruction,
            IGCodeInstruction<BinaryInstructionParameter> setAxesHardLimitInstruction,
            IGCodeInstruction<EmptyInstructionParameter> unlockAxesInstruction,
            IGCodeInstruction<EmptyInstructionParameter> sequentialHomingInstruction,
            IGCodeInstruction<EmptyInstructionParameter> simultaneousHomingInstruction,
            IGCodeInstruction<IntegerInstructionParameter> setAirPumpPwmInstruction,
            IGCodeInstruction<IntegerInstructionParameter> setGripperPwmInstruction)
        {
            this.RequestPositionInstruction = requestPositionInstruction;
            this.CartesianAbsolutePtpMoveInstrcution = cartesianAbsolutePtpMoveInstrcution;
            this.CartesianAbsoluteLinMoveInstruction = cartesianAbsoluteLinMoveInstruction;
            this.CartesianRelativePtpMoveInstruction = cartesianRelativePtpMoveInstruction;
            this.CartesianRelativeLinMoveInstruction = cartesianRelativeLinMoveInstruction;
            this.AngleAbsoluteMoveInstrcution = angleAbsoluteMoveInstruction;
            this.AngleRelativeMoveInstruction = angleRelativeMoveInstruction;
            this.SetAxesSoftLimitInstruction = setAxesSoftLimitInstruction;
            this.SetAxesHardLimitInstruction = setAxesHardLimitInstruction;
            this.UnlockAxesInstruction = unlockAxesInstruction;
            this.SequentialHomingInstruction = sequentialHomingInstruction;
            this.SimultaneousHomingInstruction = simultaneousHomingInstruction;
            this.SetAirPumpPwmInstruction = setAirPumpPwmInstruction;
            this.SetGripperPwmInstruction = setGripperPwmInstruction;
        }

        public IGCodeInstruction<EmptyInstructionParameter> RequestPositionInstruction { get; set; }

        public IGCodeInstruction<MotionInstructionParameter> CartesianAbsolutePtpMoveInstrcution { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> CartesianAbsoluteLinMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> CartesianRelativePtpMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> CartesianRelativeLinMoveInstruction { get; set; }

        public IGCodeInstruction<MotionInstructionParameter> AngleAbsoluteMoveInstrcution { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> AngleRelativeMoveInstruction { get; set; }

        public IGCodeInstruction<BinaryInstructionParameter> SetAxesSoftLimitInstruction { get; set; }
        public IGCodeInstruction<BinaryInstructionParameter> SetAxesHardLimitInstruction { get; set; }

        public IGCodeInstruction<EmptyInstructionParameter> UnlockAxesInstruction { get; set; }

        public IGCodeInstruction<EmptyInstructionParameter> SequentialHomingInstruction { get; set; }
        public IGCodeInstruction<EmptyInstructionParameter> SimultaneousHomingInstruction { get; set; }

        public IGCodeInstruction<IntegerInstructionParameter> SetAirPumpPwmInstruction { get; set; }
        public IGCodeInstruction<IntegerInstructionParameter> SetGripperPwmInstruction { get; set; }

        public IList<IGCodeInstruction> AllInstructions => new List<IGCodeInstruction>() 
        {
            this.RequestPositionInstruction,
            this.CartesianAbsolutePtpMoveInstrcution,
            this.CartesianAbsoluteLinMoveInstruction,
            this.CartesianRelativePtpMoveInstruction,
            this.CartesianRelativeLinMoveInstruction,
            this.AngleAbsoluteMoveInstrcution,
            this.AngleRelativeMoveInstruction,
            this.SetAxesSoftLimitInstruction,
            this.SetAxesHardLimitInstruction,
            this.UnlockAxesInstruction,
            this.SequentialHomingInstruction,
            this.SimultaneousHomingInstruction,
            this.SetAirPumpPwmInstruction,
            this.SetGripperPwmInstruction
        };
    }
}
