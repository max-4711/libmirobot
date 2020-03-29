namespace Libmirobot.GCode.InstructionParameters
{
    /// <summary>
    /// Represents a parameter for g code instructions, where a single integer value is being passed
    /// </summary>
    public class IntegerInstructionParameter
    {
        /// <summary>
        /// Integer value for the g code instruction.
        /// </summary>
        public int Parameter { get; set; }
    }
}
