using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to turn the air pump on or off.
    /// </summary>
    /// <remarks>Instruction no. 13 as specified in protocol specification.</remarks>
    public class SwitchAirPumpInstruction : IGCodeInstruction<IntegerInstructionParameter>
    {
        /// <inheritdoc/>
        public string UniqueIdentifier => "SWITCH_PNEUMATICPUMP";

        /// <inheritdoc/>
        public string GenerateGCode(IntegerInstructionParameter inputValue)
        {
            if (inputValue.Parameter < 0)
                inputValue.Parameter = 0;

            if (inputValue.Parameter > 1000)
                inputValue.Parameter = 1000;

            return $"M3S{inputValue.Parameter}";
        }
    }
}
