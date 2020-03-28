using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Switches the soft limit of all axes on or off.
    /// </summary>
    /// <remarks>Instruction no. 3 as specified in protocol specification.</remarks>
    public class ToggleAxesSoftLimitInstruction : IGCodeInstruction<BinaryInstructionParameter>
    {
        public string UniqueIdentifier => "SWITCH_LIMIT_SOFT";

        public string GenerateGCode(BinaryInstructionParameter inputValue)
        {
            var param = inputValue.OpenClose == true ? 1 : 0;

            return $"$20={param}";
        }
    }
}
