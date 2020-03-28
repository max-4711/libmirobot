using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to perform a homing operation (all axes sequentially).
    /// </summary>
    /// <remarks>Instruction no. 12 as specified in protocol specification.</remarks>
    public class HomingSequentialInstruction : IGCodeInstruction<EmptyInstructionParameter>
    {
        public string UniqueIdentifier => "HOMING_SEQ";

        public string GenerateGCode(EmptyInstructionParameter inputValue)
        {
            return "$HH";
        }
    }
}
