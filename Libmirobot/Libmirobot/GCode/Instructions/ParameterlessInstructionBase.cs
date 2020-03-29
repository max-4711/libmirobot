using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    internal abstract class ParameterlessInstructionBase : IGCodeInstruction<EmptyInstructionParameter>
    {
        /// <inheritdoc/>
        public abstract string UniqueIdentifier { get; set; }

        /// <inheritdoc/>
        public string GenerateGCode()
        {
            return this.GenerateGCode(new EmptyInstructionParameter());
        }

        /// <inheritdoc/>
        public abstract string GenerateGCode(EmptyInstructionParameter inputValue);

        /// <inheritdoc/>
        public abstract RobotStatusUpdate ProcessResponse(string returnValue);
    }
}
