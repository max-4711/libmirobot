using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libmirobotTests.GCodeInstructions
{
    [TestClass]
    public class HomingSimultaneousInstructionShould
    {
        [TestMethod]
        public void GenerateSingleHAsGCode()
        {
            var testObject = new HomingSimultaneousInstruction();

            var testResult = testObject.GenerateGCode(new EmptyInstructionParameter());

            Assert.AreEqual("$H", testResult);
        }
    }
}
