namespace Libmirobot.GCode
{
    public interface IGCodeInstruction<TInput, TReturn>
    {
        string GenerateGCode(TInput inputValue);

        TReturn ProcessResponse(string returnValue);
    }
}
