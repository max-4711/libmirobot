using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libmirobotTests.GCodeInstructions
{
    [TestClass]
    public class ToggleAxesSoftLimitInstructionShould
    {
        [TestMethod]
        public void SetParameterValue1IfBooleanIsTrue()
        {
            var testObject = new ToggleAxesSoftLimitInstruction();

            var testResult = testObject.GenerateGCode(new BinaryInstructionParameter { OpenClose = true });

            Assert.AreEqual("$20=1", testResult);
        }

        [TestMethod]
        public void SetParameterValue0IfBooleanIsFalse()
        {
            var testObject = new ToggleAxesSoftLimitInstruction();

            var testResult = testObject.GenerateGCode(new BinaryInstructionParameter { OpenClose = false });

            Assert.AreEqual("$20=0", testResult);
        }
    }
}
