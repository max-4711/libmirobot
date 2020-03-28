using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Obtains the status and position information of the current manipulator.
    /// </summary>
    /// <remarks>Instruction no. 1 as specified in protocol specification.</remarks>
    public class ObtainStatusInstruction : IGCodeInstruction<EmptyInstructionParameter>
    {
        public string UniqueIdentifier => "GET_STATUS_ALL";

        public bool CanProcessResponse(string response)
        {
            return response.StartsWith("<Idle,Angle(ABCDXYZ):") && response.Contains(",Cartesian coordinate(XYZRxRyRz):") && response.Contains(",Pump PWM:") && response.Contains(",Value PWM:") && response.EndsWith(">");
        }

        public string GenerateGCode(EmptyInstructionParameter inputValue)
        {
            return "?";
        }

        public RobotStatusUpdate ProcessResponse(string returnValue)
        {
            if (string.IsNullOrEmpty(returnValue))
                return new RobotStatusUpdate();

            try
            {
                //<Idle,Angle(ABCDXYZ):{A},{B},{C},{D},{X},{Y},{Z},Cartesian coordinate(XYZRxRyRz):{X},{Y},{Z},{Rx},{Ry},{Rz},Pump PWM:{PWM1},Value PWM:{PWM2}>

                var colonSeparatedStrings = returnValue.Split(':');

                var anglesCommaSeparated = colonSeparatedStrings[1].Split(',');
                var angle1 = double.Parse(anglesCommaSeparated[0]);
                var angle2 = double.Parse(anglesCommaSeparated[1]);
                var angle3 = double.Parse(anglesCommaSeparated[2]);
                double? slideRail = null;
                double angle4;
                double angle5;
                double angle6;
                if (anglesCommaSeparated.Length == 8)
                {
                    slideRail = double.Parse(anglesCommaSeparated[3]);
                    angle4 = double.Parse(anglesCommaSeparated[4]);
                    angle5 = double.Parse(anglesCommaSeparated[5]);
                    angle6 = double.Parse(anglesCommaSeparated[6]);
                }
                else
                {
                    angle4 = double.Parse(anglesCommaSeparated[3]);
                    angle5 = double.Parse(anglesCommaSeparated[4]);
                    angle6 = double.Parse(anglesCommaSeparated[5]);
                }

                var cartesianCommaSeparated = colonSeparatedStrings[2].Split(',');
                var xCoordinate = double.Parse(cartesianCommaSeparated[0]);
                var yCoordinate = double.Parse(cartesianCommaSeparated[1]);
                var zCoordinate = double.Parse(cartesianCommaSeparated[2]);
                var xRotation = double.Parse(cartesianCommaSeparated[3]);
                var yRotation = double.Parse(cartesianCommaSeparated[4]);
                var zRotation = double.Parse(cartesianCommaSeparated[5]);

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
