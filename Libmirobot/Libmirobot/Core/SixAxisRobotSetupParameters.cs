using Libmirobot.GCode;
using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.ReturnValues;

namespace Libmirobot.Core
{
    internal class SixAxisRobotSetupParameters
    {
        public IGCodeInstruction<EmptyInstructionParameter, StatusReturnValue> RequestPositionInstruction { get; set; }

        public IGCodeInstruction<MotionInstructionParameter, EmptyReturnValue> CartesianAbsolutePtpMoveInstrcution { get; set; }
        public IGCodeInstruction<MotionInstructionParameter, EmptyReturnValue> CartesianAbsoluteLinMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter, EmptyReturnValue> CartesianRelativePtpMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter, EmptyReturnValue> CartesianRelativeLinMoveInstruction { get; set; }

        public IGCodeInstruction<MotionInstructionParameter, EmptyReturnValue> AngleAbsolutePtpMoveInstrcution { get; set; }
        public IGCodeInstruction<MotionInstructionParameter, EmptyReturnValue> AngleAbsoluteLinMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter, EmptyReturnValue> AngleRelativePtpMoveInstruction { get; set; }
        public IGCodeInstruction<MotionInstructionParameter, EmptyReturnValue> AngleRelativeLinMoveInstruction { get; set; }

        public IGCodeInstruction<BinaryInstructionParameter, EmptyReturnValue> SetAxesSoftLimitInstruction { get; set; }
        public IGCodeInstruction<BinaryInstructionParameter, EmptyReturnValue> SetAxesHardLimitInstruction { get; set; }

        public IGCodeInstruction<EmptyInstructionParameter, EmptyReturnValue> UnlockAxesInstruction { get; set; }

        public IGCodeInstruction<EmptyInstructionParameter, EmptyReturnValue> SequentialHomingInstruction { get; set; }
        public IGCodeInstruction<EmptyInstructionParameter, EmptyReturnValue> SimultaneousHomingInstruction { get; set; }

        public IGCodeInstruction<IntegerInstructionParameter, EmptyReturnValue> SetAirPumpPwmInstruction { get; set; }
        public IGCodeInstruction<IntegerInstructionParameter, EmptyReturnValue> SetGripperPwmInstruction { get; set; }
    }
}
