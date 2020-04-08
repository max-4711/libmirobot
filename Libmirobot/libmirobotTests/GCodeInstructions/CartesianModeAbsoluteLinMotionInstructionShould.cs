using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libmirobotTests.GCodeInstructions
{
    [TestClass]
    public class CartesianModeAbsoluteLinMotionInstructionShould
    {
        [TestMethod]
        public void ProduceCorrectlyFormattedGCode()
        {
            var testObject = new CartesianModeAbsoluteLinMotionInstruction();

            var testResult = testObject.GenerateGCode(new MotionInstructionParameter
            {
                PositioningParameter1 = 1.1f,
                PositioningParameter2 = 2.2f,
                PositioningParameter3 = 3.3f,
                PositioningParameter4 = 4.4f,
                PositioningParameter5 = 5.5f,
                PositioningParameter6 = 6.6f,
                Speed = 2000
            });

            Assert.AreEqual("M20 G90 G1 X1.1 Y2.2 Z3.3 A4.4 B5.5 C6.6 F2000", testResult);
        }
    }
}
