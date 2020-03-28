using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace libmirobotTests.GCodeInstructions
{
    [TestClass]
    public class ObtainStatusInstructionShould
    {
        const double angle1 = 50.1;
        const double angle2 = 51.2;
        const double angle3 = 52.3;
        const double angle4 = 53.4;
        const double angle5 = 54.5;
        const double angle6 = 55.6;
        const double slideRail = 56.7;
        const double xCoordinate = 15.1;
        const double yCoordinate = 16.2;
        const double zCoordinate = 17.3;
        const double xRotation = 18.4;
        const double yRotation = 19.5;
        const double zRotation = 20.6;
        const int pumpPwm = 500;
        const int gripperPwm = 50;

        string sampleRobotResponse = string.Format(CultureInfo.InvariantCulture, "<Idle,Angle(ABCDXYZ):{0},{1},{2},{3},{4},{5},{6},Cartesian coordinate(XYZRxRyRz):{7},{8},{9},{10},{11},{12},Pump PWM:{13},Value PWM:{14}>", angle1, angle2, angle3, slideRail, angle4, angle5, angle6, xCoordinate, yCoordinate, zCoordinate, xRotation, yRotation, zRotation, pumpPwm, gripperPwm);

        [TestMethod]
        [Description("ObtainStatusInstruction should always generate '?' as g code instruction.")]
        public void GenerateQuestionMarkAsGCode()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.GenerateGCode(new EmptyInstructionParameter());

            Assert.AreEqual("?", testResult);
        }

        [TestMethod]
        [Description("ObtainStatusInstruction should parse the command response into a robot status information object")]
        public void ParseResponseToStatusInformationObject()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.ProcessResponse(sampleRobotResponse);

            Assert.IsTrue(testResult.HasData);
            Assert.AreEqual(angle1, testResult.Axis1Angle);
            Assert.AreEqual(angle2, testResult.Axis2Angle);
            Assert.AreEqual(angle3, testResult.Axis3Angle);
            Assert.AreEqual(angle4, testResult.Axis4Angle);
            Assert.AreEqual(angle5, testResult.Axis5Angle);
            Assert.AreEqual(angle6, testResult.Axis6Angle);
            Assert.AreEqual(slideRail, testResult.ExternalSlideRail);
            Assert.AreEqual(xCoordinate, testResult.XCoordinate);
            Assert.AreEqual(yCoordinate, testResult.YCoordinate);
            Assert.AreEqual(zCoordinate, testResult.ZCoordinate);
            Assert.AreEqual(xRotation, testResult.XRotation);
            Assert.AreEqual(yRotation, testResult.YRotation);
            Assert.AreEqual(zRotation, testResult.ZRotation);
            Assert.AreEqual(pumpPwm, testResult.Pwm1);
            Assert.AreEqual(gripperPwm, testResult.Pwm2);
        }

        [TestMethod]
        [Description("ObtainStatusInstruction should be able to process a sample response from the robot")]
        public void BeAbleToProcessRegularResponse()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.CanProcessResponse(sampleRobotResponse);

            Assert.IsTrue(testResult);
        }

        [TestMethod]
        [Description("ObtainStatusInstruction should not be able to process bogus response")]
        public void NotBeAbleToProcessBogusResponse()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.CanProcessResponse("ok");

            Assert.IsFalse(testResult);
        }
    }
}
