using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Obtains the status and position information of the current manipulator
    /// </summary>
    /// <remarks>Instruction no. 1 as specified in protocol specification</remarks>
    public class ObtainStatusInstruction : IGCodeInstruction<EmptyInstructionParameter>
    {
        public string UniqueIdentifier => "GET_STATUS_ALL";

        public string GenerateGCode(EmptyInstructionParameter inputValue)
        {
            return "?";
        }

        public RobotStatusUpdate ProcessResponse(string returnValue)
        {
            throw new System.NotImplementedException();
        }
    }
}
