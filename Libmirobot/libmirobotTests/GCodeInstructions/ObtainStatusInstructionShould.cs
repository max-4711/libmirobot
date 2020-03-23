using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libmirobotTests.GCodeInstructions
{
    [TestClass]
    public class ObtainStatusInstructionShould
    {
        [TestMethod]
        [Description("ObtainStatusInstruction should always generate '?' as g code instruction.")]
        public void GenerateQuestionMarkAsGCode()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.GenerateGCode(new EmptyInstructionParameter());

            Assert.AreEqual("?", testResult);
        }
    }
}
