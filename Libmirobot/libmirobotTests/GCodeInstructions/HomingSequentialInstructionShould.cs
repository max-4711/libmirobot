using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libmirobotTests.GCodeInstructions
{
    [TestClass]
    public class HomingSequentialInstructionShould
    {
        [TestMethod]
        public void GenerateDoubleHAsGCode()
        {
            var testObject = new HomingSequentialInstruction();

            var testResult = testObject.GenerateGCode(new EmptyInstructionParameter());

            Assert.AreEqual("$HH", testResult);
        }
    }
}
