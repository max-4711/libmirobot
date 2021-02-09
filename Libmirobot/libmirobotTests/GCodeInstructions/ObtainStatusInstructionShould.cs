using Libmirobot.GCode.InstructionParameters;
using Libmirobot.GCode.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace libmirobotTests.GCodeInstructions
{
    [TestClass]
    public class ObtainStatusInstructionShould
    {
        const double angleA = 50.1;
        const double angleB = 51.2;
        const double angleC = 52.3;
        const double angleX = 53.4;
        const double angleY = 54.5;
        const double angleZ = 55.6;
        const double slideRail = 56.7;
        const double xCoordinate = 15.1;
        const double yCoordinate = 16.2;
        const double zCoordinate = 17.3;
        const double xRotation = 18.4;
        const double yRotation = 19.5;
        const double zRotation = 20.6;
        const int pumpPwm = 500;
        const int gripperPwm = 50;

        readonly string sampleRobotIdleResponse = string.Format(CultureInfo.InvariantCulture, "<Idle,Angle(ABCDXYZ):{0},{1},{2},{3},{4},{5},{6},Cartesian coordinate(XYZRxRyRz):{7},{8},{9},{10},{11},{12},Pump PWM:{13},Valve PWM:{14},Motion_MODE:0>", angleA, angleB, angleC, slideRail, angleX, angleY, angleZ, xCoordinate, yCoordinate, zCoordinate, xRotation, yRotation, zRotation, pumpPwm, gripperPwm);
        readonly string sampleRobotBusyResponse = string.Format(CultureInfo.InvariantCulture, "<Run,Angle(ABCDXYZ):{0},{1},{2},{3},{4},{5},{6},Cartesian coordinate(XYZRxRyRz):{7},{8},{9},{10},{11},{12},Pump PWM:{13},Valve PWM:{14},Motion_MODE:0>", angleA, angleB, angleC, slideRail, angleX, angleY, angleZ, xCoordinate, yCoordinate, zCoordinate, xRotation, yRotation, zRotation, pumpPwm, gripperPwm);

        [TestMethod]
        [Description("ObtainStatusInstruction should always generate '?' as g code instruction.")]
        public void GenerateQuestionMarkAsGCode()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.GenerateGCode(new EmptyInstructionParameter());

            Assert.AreEqual("?", testResult);
        }

        [TestMethod]
        [Description("ObtainStatusInstruction should parse the command response of a robot being idle into a robot status information object")]
        public void ParseIdleResponseToStatusInformationObject()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.ProcessResponse(sampleRobotIdleResponse);

            Assert.IsTrue(testResult.HasData);
            Assert.IsTrue(testResult.IsIdle.Value);
            Assert.AreEqual(angleX, testResult.Axis1Angle);
            Assert.AreEqual(angleY, testResult.Axis2Angle);
            Assert.AreEqual(angleZ, testResult.Axis3Angle);
            Assert.AreEqual(angleA, testResult.Axis4Angle);
            Assert.AreEqual(angleB, testResult.Axis5Angle);
            Assert.AreEqual(angleC, testResult.Axis6Angle);
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
        [Description("ObtainStatusInstruction should parse the command response of a robot being idle into a robot status information object")]
        public void ParseBusyResponseToStatusInformationObject()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.ProcessResponse(sampleRobotBusyResponse);

            Assert.IsTrue(testResult.HasData);
            Assert.IsFalse(testResult.IsIdle.Value);
            Assert.AreEqual(angleX, testResult.Axis1Angle);
            Assert.AreEqual(angleY, testResult.Axis2Angle);
            Assert.AreEqual(angleZ, testResult.Axis3Angle);
            Assert.AreEqual(angleA, testResult.Axis4Angle);
            Assert.AreEqual(angleB, testResult.Axis5Angle);
            Assert.AreEqual(angleC, testResult.Axis6Angle);
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
        [Description("ObtainStatusInstruction should be able to process a sample response while being idle from the robot")]
        public void BeAbleToProcessIdleResponse()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.CanProcessResponse(sampleRobotIdleResponse);

            Assert.IsTrue(testResult);
        }

        [TestMethod]
        [Description("ObtainStatusInstruction should be able to process a sample response while being in motion from the robot")]
        public void BeAbleToProcessInMotionResponse()
        {
            var testObject = new ObtainStatusInstruction();

            var testResult = testObject.CanProcessResponse(sampleRobotBusyResponse);

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
