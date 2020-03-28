namespace Libmirobot.GCode
{
    public interface IGCodeInstruction
    {
        string UniqueIdentifier { get; }

        RobotStatusUpdate ProcessResponse(string returnValue);
    }

    public interface IGCodeInstruction<TInput> : IGCodeInstruction
    {
        string GenerateGCode(TInput inputValue);        
    }
}
