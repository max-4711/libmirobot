using Libmirobot.GCode.InstructionParameters;
using System.Globalization;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Obtains the status and position information of the current manipulator.
    /// </summary>
    /// <remarks>Instruction no. 1 as specified in protocol specification.</remarks>
    public class ObtainStatusInstruction : IGCodeInstruction<EmptyInstructionParameter>
    {
        /// <inheritdoc/>
        public string UniqueIdentifier => "GET_STATUS_ALL";

        /// <inheritdoc/>
        public bool IsMotionInstruction => false;

        /// <inheritdoc/>
        public bool CanProcessResponse(string response)
        {
            var responseStartFits = response.StartsWith("<Idle,Angle(ABCDXYZ):") || response.StartsWith("<Run,Angle(ABCDXYZ):") || response.StartsWith("<Home,Angle(ABCDXYZ):");
            var responseContainsCartesianCoordinateString = response.Contains(",Cartesian coordinate(XYZRxRyRz):") || response.Contains(",Cartesian coordinate(XYZ RxRyRz):");
            var responseContainsPwmStrings = response.Contains(",Pump PWM:") && response.Contains(",Valve PWM:");
            var responseEndFits = response.TrimEnd('\r').EndsWith(">");

            return responseStartFits && responseContainsCartesianCoordinateString && responseContainsPwmStrings && responseEndFits;
        }

        /// <inheritdoc/>
        public string GenerateGCode(EmptyInstructionParameter inputValue)
        {
            return "?";
        }

        /// <inheritdoc/>
        public RobotStatusUpdate ProcessResponse(string returnValue)
        {
            if (string.IsNullOrEmpty(returnValue))
                return new RobotStatusUpdate();

            try
            {
                //<Idle,Angle(ABCDXYZ):{A},{B},{C},{D},{X},{Y},{Z},Cartesian coordinate(XYZRxRyRz):{X},{Y},{Z},{Rx},{Ry},{Rz},Pump PWM:{PWM1},Valve PWM:{PWM2}>

                var colonSeparatedStrings = returnValue.Split(':');

                var idleState = colonSeparatedStrings[0].Split(',');
                var idleStateTrimmed = idleState[0].TrimStart('<');

                bool isIdle = false;
                if (idleStateTrimmed == "Idle")
                {
                    isIdle = true;
                }

                var anglesCommaSeparated = colonSeparatedStrings[1].Split(',');
                var angleA = double.Parse(anglesCommaSeparated[0], CultureInfo.InvariantCulture.NumberFormat);
                var angleB = double.Parse(anglesCommaSeparated[1], CultureInfo.InvariantCulture.NumberFormat);
                var angleC = double.Parse(anglesCommaSeparated[2], CultureInfo.InvariantCulture.NumberFormat);
                double? slideRail = null;
                double angleX;
                double angleY;
                double angleZ;
                if (anglesCommaSeparated.Length == 8)
                {
                    slideRail = double.Parse(anglesCommaSeparated[3], CultureInfo.InvariantCulture.NumberFormat);
                    angleX = double.Parse(anglesCommaSeparated[4], CultureInfo.InvariantCulture.NumberFormat);
                    angleY = double.Parse(anglesCommaSeparated[5], CultureInfo.InvariantCulture.NumberFormat);
                    angleZ = double.Parse(anglesCommaSeparated[6], CultureInfo.InvariantCulture.NumberFormat);
                }
                else
                {
                    angleX = double.Parse(anglesCommaSeparated[3], CultureInfo.InvariantCulture.NumberFormat);
                    angleY = double.Parse(anglesCommaSeparated[4], CultureInfo.InvariantCulture.NumberFormat);
                    angleZ = double.Parse(anglesCommaSeparated[5], CultureInfo.InvariantCulture.NumberFormat);
                }

                var cartesianCommaSeparated = colonSeparatedStrings[2].Split(',');
                var xCoordinate = double.Parse(cartesianCommaSeparated[0], CultureInfo.InvariantCulture.NumberFormat);
                var yCoordinate = double.Parse(cartesianCommaSeparated[1], CultureInfo.InvariantCulture.NumberFormat);
                var zCoordinate = double.Parse(cartesianCommaSeparated[2], CultureInfo.InvariantCulture.NumberFormat);
                var xRotation = double.Parse(cartesianCommaSeparated[3], CultureInfo.InvariantCulture.NumberFormat);
                var yRotation = double.Parse(cartesianCommaSeparated[4], CultureInfo.InvariantCulture.NumberFormat);
                var zRotation = double.Parse(cartesianCommaSeparated[5], CultureInfo.InvariantCulture.NumberFormat);

                var pumpPwmCommaSeparated = colonSeparatedStrings[3].Split(',');
                var pumpPwm = int.Parse(pumpPwmCommaSeparated[0]);

                var gripperPwmCommaSeparated = colonSeparatedStrings[4].Split(','); //The example in the specification lists a value "Motion_MODE" which is not included in the aforementioned string schematics. Just making sure the code is fine with both variants.
                var gripperPwmString = gripperPwmCommaSeparated[0].TrimEnd('>');
                var gripperPwm = int.Parse(gripperPwmString);


                //Angle(ABCDXYZ):
                //A = A4
                //B = A5
                //C = A6
                //D = Slide rail
                //X = A1
                //Y = A2
                //Z = A3
                return new RobotStatusUpdate
                {
                    IsIdle = isIdle,
                    Axis1Angle = angleX,
                    Axis2Angle = angleY,
                    Axis3Angle = angleZ,
                    Axis4Angle = angleA,
                    Axis5Angle = angleB,
                    Axis6Angle = angleC,
                    ExternalSlideRail = slideRail,
                    Pwm1 = pumpPwm,
                    Pwm2 = gripperPwm,
                    XCoordinate = xCoordinate,
                    YCoordinate = yCoordinate,
                    ZCoordinate = zCoordinate,
                    XRotation = xRotation,
                    YRotation = yRotation,
                    ZRotation = zRotation
                };
            }
            catch
            {
                return new RobotStatusUpdate();
            }            
        }
    }
}
