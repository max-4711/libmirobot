using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to perform a homing operation (all axes sequentially).
    /// </summary>
    /// <remarks>Instruction no. 12 as specified in protocol specification.</remarks>
    public class HomingSequentialInstruction : IGCodeInstruction<EmptyInstructionParameter>
    {
        /// <inheritdoc/>
        public string UniqueIdentifier => "HOMING_SEQ";

        /// <inheritdoc/>
        public bool IsMotionInstruction => true;

        /// <inheritdoc/>
        public string GenerateGCode(EmptyInstructionParameter inputValue)
        {
            return "$HH";
        }
    }
}
