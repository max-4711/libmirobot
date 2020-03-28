using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libmirobotTests.GCodeInstructions
{
    [TestClass]
    public class ToggleAxesHardLimitInstructionShould
    {
        [TestMethod]
        public void SetParameterValue1IfBooleanIsTrue()
        {
            var testObject = new ToggleAxesHardLimitInstruction();

            var testResult = testObject.GenerateGCode(new BinaryInstructionParameter { OpenClose = true });

            Assert.AreEqual("$21=1", testResult);
        }

        [TestMethod]
        public void SetParameterValue0IfBooleanIsFalse()
        {
            var testObject = new ToggleAxesHardLimitInstruction();

            var testResult = testObject.GenerateGCode(new BinaryInstructionParameter { OpenClose = false });

            Assert.AreEqual("$21=0", testResult);
        }
    }
}
