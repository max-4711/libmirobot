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
        public bool CanProcessResponse(string response)
        {
            return response.StartsWith("<Idle,Angle(ABCDXYZ):") && response.Contains(",Cartesian coordinate(XYZRxRyRz):") && response.Contains(",Pump PWM:") && response.Contains(",Value PWM:") && response.EndsWith(">");
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
                //<Idle,Angle(ABCDXYZ):{A},{B},{C},{D},{X},{Y},{Z},Cartesian coordinate(XYZRxRyRz):{X},{Y},{Z},{Rx},{Ry},{Rz},Pump PWM:{PWM1},Value PWM:{PWM2}>

                var colonSeparatedStrings = returnValue.Split(':');

                var anglesCommaSeparated = colonSeparatedStrings[1].Split(',');
                var angle1 = double.Parse(anglesCommaSeparated[0], CultureInfo.InvariantCulture.NumberFormat);
                var angle2 = double.Parse(anglesCommaSeparated[1], CultureInfo.InvariantCulture.NumberFormat);
                var angle3 = double.Parse(anglesCommaSeparated[2], CultureInfo.InvariantCulture.NumberFormat);
                double? slideRail = null;
                double angle4;
                double angle5;
                double angle6;
                if (anglesCommaSeparated.Length == 8)
                {
                    slideRail = double.Parse(anglesCommaSeparated[3], CultureInfo.InvariantCulture.NumberFormat);
                    angle4 = double.Parse(anglesCommaSeparated[4], CultureInfo.InvariantCulture.NumberFormat);
                    angle5 = double.Parse(anglesCommaSeparated[5], CultureInfo.InvariantCulture.NumberFormat);
                    angle6 = double.Parse(anglesCommaSeparated[6], CultureInfo.InvariantCulture.NumberFormat);
                }
                else
                {
                    angle4 = double.Parse(anglesCommaSeparated[3], CultureInfo.InvariantCulture.NumberFormat);
                    angle5 = double.Parse(anglesCommaSeparated[4], CultureInfo.InvariantCulture.NumberFormat);
                    angle6 = double.Parse(anglesCommaSeparated[5], CultureInfo.InvariantCulture.NumberFormat);
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

                return new RobotStatusUpdate
                {
                    Axis1Angle = angle1,
                    Axis2Angle = angle2,
                    Axis3Angle = angle3,
                    Axis4Angle = angle4,
                    Axis5Angle = angle5,
                    Axis6Angle = angle6,
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
