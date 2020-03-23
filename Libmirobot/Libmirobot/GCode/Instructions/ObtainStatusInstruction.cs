using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.ReturnValues;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Obtains the status and position information of the current manipulator
    /// </summary>
    /// <remarks>Instruction no. 1 as specified in protocol specification</remarks>
    public class ObtainStatusInstruction : IGCodeInstruction<EmptyInstructionParameter, StatusReturnValue>
    {
        public string GenerateGCode(EmptyInstructionParameter inputValue)
        {
            return "?";
        }

        public StatusReturnValue ProcessResponse(string returnValue)
        {
            throw new System.NotImplementedException();
        }
    }
}
