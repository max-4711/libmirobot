using Libmirobot.GCode.InstructionParameters;
using System.Globalization;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to increment/decrement its current position in cartesian mode (linear interpolation).
    /// </summary>
    /// <remarks>Instruction no. 10 as specified in protocol specification.</remarks>
    public class CartesianModeRelativeLinMotionInstruction : IGCodeInstruction<MotionInstructionParameter>
    {
        public string UniqueIdentifier => "MOTION_CARTESIAN_RELATIVE_LIN";

        public string GenerateGCode(MotionInstructionParameter inputValue)
        {
            return string.Format(CultureInfo.InvariantCulture, "M20 G91 G1 X{0} Y{1} Z{2} A{3} B{4} C{5} F{6}", inputValue.PositioningParameter1, inputValue.PositioningParameter2, inputValue.PositioningParameter3, inputValue.PositioningParameter4, inputValue.PositioningParameter5, inputValue.PositioningParameter6, inputValue.Speed);
        }
    }
}
