using Libmirobot.GCode;
using Libmirobot.GCode.InstructionParameters;
using System.Collections.Generic;

namespace Libmirobot.Core
{
    internal class SixAxisRobotSetupParameters
    {
        public IGCodeInstruction<EmptyInstructionParameter> RequestPositionInstruction { get; set; }

        public IGCodeInstruction<MotionInstructionParameter> CartesianAbsolutePtpMoveInstrcution { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> CartesianAbsoluteLinMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> CartesianRelativePtpMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> CartesianRelativeLinMoveInstruction { get; set; }

        public IGCodeInstruction<MotionInstructionParameter> AngleAbsolutePtpMoveInstrcution { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> AngleAbsoluteLinMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> AngleRelativePtpMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter> AngleRelativeLinMoveInstruction { get; set; }

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
            this.AngleAbsolutePtpMoveInstrcution,
            this.AngleAbsoluteLinMoveInstruction,
            this.AngleRelativePtpMoveInstruction,
            this.AngleRelativeLinMoveInstruction,
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
