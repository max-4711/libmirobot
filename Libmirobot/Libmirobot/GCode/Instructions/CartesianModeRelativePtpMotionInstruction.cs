using Libmirobot.GCode.InstructionParameters;
using System.Globalization;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to increment/decrement its current position in cartesian mode (point-to-point).
    /// </summary>
    /// <remarks>Instruction no. 9 as specified in protocol specification.</remarks>
    public class CartesianModeRelativePtpMotionInstruction : IGCodeInstruction<MotionInstructionParameter>
    {
        /// <inheritdoc/>
        public string UniqueIdentifier => "MOTION_CARTESIAN_RELATIVE_PTP";

        /// <inheritdoc/>
        public string GenerateGCode(MotionInstructionParameter inputValue)
        {
            return string.Format(CultureInfo.InvariantCulture, "M20 G91 G0 X{0} Y{1} Z{2} A{3} B{4} C{5} F{6}", inputValue.PositioningParameter1, inputValue.PositioningParameter2, inputValue.PositioningParameter3, inputValue.PositioningParameter4, inputValue.PositioningParameter5, inputValue.PositioningParameter6, inputValue.Speed);
        }
    }
}
