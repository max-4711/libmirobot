namespace Libmirobot.GCode
{
    public interface IGCodeInstruction
    {
        string UniqueIdentifier { get; }

        bool CanProcessResponse(string response)
        {
            return false;
        }

        RobotStatusUpdate ProcessResponse(string returnValue)
        {
            return new RobotStatusUpdate();
        }
    }

    public interface IGCodeInstruction<TInput> : IGCodeInstruction
    {
        string GenerateGCode(TInput inputValue);        
    }
}
