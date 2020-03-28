namespace Libmirobot.GCode
{
    public interface IGCodeInstruction<TInput, TReturn>
    {
        string UniqueIdentifier { get; }

        string GenerateGCode(TInput inputValue);

        TReturn ProcessResponse(string returnValue);
    }
}
