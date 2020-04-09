using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Releases the shaft locking state of all axes.
    /// </summary>
    /// <remarks>Instruction no. 2 as specified in protocol specification.</remarks>
    public class UnlockAxesInstruction : IGCodeInstruction<EmptyInstructionParameter>
    {
        /// <inheritdoc/>
        public string UniqueIdentifier => "UNLOCK_AXES_ALL";

        /// <inheritdoc/>
        public bool IsMotionInstruction => false;

        /// <inheritdoc/>
        public string GenerateGCode(EmptyInstructionParameter inputValue)
        {
            return "M50";
        }
    }
}
