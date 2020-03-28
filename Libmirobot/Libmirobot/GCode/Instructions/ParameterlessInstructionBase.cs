using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    public abstract class ParameterlessInstructionBase : IGCodeInstruction<EmptyInstructionParameter>
    {
        public abstract string UniqueIdentifier { get; set; }

        public string GenerateGCode()
        {
            return this.GenerateGCode(new EmptyInstructionParameter());
        }

        public abstract string GenerateGCode(EmptyInstructionParameter inputValue);

        public abstract RobotStatusUpdate ProcessResponse(string returnValue);
    }
}
