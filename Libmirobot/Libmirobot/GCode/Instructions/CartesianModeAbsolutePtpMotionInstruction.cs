using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to move to the specified absolute position in cartesian mode (point-to-point).
    /// </summary>
    /// <remarks>Instruction no. 7 as specified in protocol specification.</remarks>
    public class CartesianModeAbsolutePtpMotionInstruction : IGCodeInstruction<MotionInstructionParameter>
    {
        public string UniqueIdentifier => "MOTION_CARTESIAN_ABSOLUTE_PTP";

        public string GenerateGCode(MotionInstructionParameter inputValue)
        {
            return $"M20 G90 G0 X{inputValue.PositioningParameter1} Y{inputValue.PositioningParameter2} Z{inputValue.PositioningParameter3} A{inputValue.PositioningParameter4} B{inputValue.PositioningParameter5} C{inputValue.PositioningParameter6} F{inputValue.Speed}";
        }
    }
}
