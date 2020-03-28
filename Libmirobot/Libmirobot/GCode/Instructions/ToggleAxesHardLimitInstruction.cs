using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Switches the hard limit of axes 1-3 on or off.
    /// </summary>
    /// <remarks>Instruction no. 4 as specified in protocol specification.</remarks>
    public class ToggleAxesHardLimitInstruction : IGCodeInstruction<BinaryInstructionParameter>
    {
        public string UniqueIdentifier => "SWITCH_LIMIT_HARD";

        public string GenerateGCode(BinaryInstructionParameter inputValue)
        {
            var param = inputValue.OpenClose == true ? 1 : 0;

            return $"$21={param}";
        }

        public RobotStatusUpdate ProcessResponse(string returnValue)
        {
            return new RobotStatusUpdate();
        }
    }
}
