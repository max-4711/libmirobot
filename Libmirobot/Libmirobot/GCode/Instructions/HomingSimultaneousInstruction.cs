using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to perform a homing operation (all axes simultaneously).
    /// </summary>
    /// <remarks>Instruction no. 11 as specified in protocol specification.</remarks>
    public class HomingSimultaneousInstruction : IGCodeInstruction<EmptyInstructionParameter>
    {
        /// <inheritdoc/>
        public string UniqueIdentifier => "HOMING_SIM";

        /// <inheritdoc/>
        public bool IsMotionInstruction => true;

        /// <inheritdoc/>
        public string GenerateGCode(EmptyInstructionParameter inputValue)
        {
            return "$H";
        }
    }
}
