namespace Libmirobot.GCode
{
    /// <summary>
    /// Represents a g code instruction.
    /// </summary>
    public interface IGCodeInstruction
    {
        /// <summary>
        /// Identifier to uniquely identify the instruction.
        /// </summary>
        string UniqueIdentifier { get; }

        /// <summary>
        /// Return if the instruction is able to process the response from the hardware.
        /// </summary>
        /// <param name="response">Response received from the hardware</param>
        /// <returns>true, if this instruction can process the response or false, if the instruction is not able to process the response.</returns>
        bool CanProcessResponse(string response)
        {
            return false;
        }

        /// <summary>
        /// Parses the response from the hardware to a robot status update object.
        /// </summary>
        /// <param name="returnValue">Response received from the hardware</param>
        /// <returns>Robot status object extracted from the hardware response.</returns>
        RobotStatusUpdate ProcessResponse(string returnValue)
        {
            return new RobotStatusUpdate();
        }
    }

    /// <summary>
    /// Represents a g code instruction with different kinds of input parameters.
    /// </summary>
    /// <typeparam name="TInput">Type of the input parameters needed for g code generation.</typeparam>
    public interface IGCodeInstruction<TInput> : IGCodeInstruction
    {
        /// <summary>
        /// Generated g code from the input parameters.
        /// </summary>
        /// <param name="inputValue">Parameters for g code generation.</param>
        /// <returns>G code, which can be sent to the hardware.</returns>
        string GenerateGCode(TInput inputValue);        
    }
}
