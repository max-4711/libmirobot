using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    public abstract class ParameterlessInstructionBase<TReturn> : IGCodeInstruction<EmptyInstructionParameter, TReturn>
    {
        public abstract string UniqueIdentifier { get; set; }

        public string GenerateGCode()
        {
            return this.GenerateGCode(new EmptyInstructionParameter());
        }

        public abstract string GenerateGCode(EmptyInstructionParameter inputValue);

        public abstract TReturn ProcessResponse(string returnValue);
    }
}
