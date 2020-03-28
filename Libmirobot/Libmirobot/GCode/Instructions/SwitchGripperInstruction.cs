using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to open or close the gripper.
    /// </summary>
    /// <remarks>Instruction no. 14 as specified in protocol specification.</remarks>
    public class SwitchGripperInstruction : IGCodeInstruction<IntegerInstructionParameter>
    {
        public string UniqueIdentifier => "SWITCH_GRIPPER";

        public string GenerateGCode(IntegerInstructionParameter inputValue)
        {
            if (inputValue.Parameter > 65)
                inputValue.Parameter = 65;

            if (inputValue.Parameter < 40)
                inputValue.Parameter = 40;

            return $"M4E{inputValue.Parameter}";
        }

        public RobotStatusUpdate ProcessResponse(string returnValue)
        {
            return new RobotStatusUpdate();
        }
    }
}
