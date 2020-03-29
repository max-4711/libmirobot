namespace Libmirobot.GCode.InstructionParameters
{
    /// <summary>
    /// Represents a parameter for g code instructions, where a single binary value is being passed
    /// </summary>
    public class BinaryInstructionParameter
    {
        /// <summary>
        /// Binary value for the g code instruction.
        /// </summary>
        public bool OpenClose { get; set; }
    }
}
