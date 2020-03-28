using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libmirobotTests.GCodeInstructions
{
    [TestClass]
    public class UnlockAxisInstructionShould
    {
        [TestMethod]
        public void GenerateM50AsGCode()
        {
            var testObject = new UnlockAxesInstruction();

            var testResult = testObject.GenerateGCode(new EmptyInstructionParameter());

            Assert.AreEqual("M50", testResult);
        }
    }
}
