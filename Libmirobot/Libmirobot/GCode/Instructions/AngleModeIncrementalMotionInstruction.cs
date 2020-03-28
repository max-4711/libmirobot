using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to increment/decrement its current position in angle mode.
    /// </summary>
    /// <remarks>Instruction no. 6 as specified in protocol specification.</remarks>
    public class AngleModeIncrementalMotionInstruction : IGCodeInstruction<MotionInstructionParameter>
    {
        public string UniqueIdentifier => "MOTION_ANGLE_INCREMENTAL";

        public string GenerateGCode(MotionInstructionParameter inputValue)
        {
            return $"M21 G91 X{inputValue.PositioningParameter1} Y{inputValue.PositioningParameter2} Z{inputValue.PositioningParameter3} A{inputValue.PositioningParameter4} B{inputValue.PositioningParameter5} C{inputValue.PositioningParameter6} F{inputValue.Speed}";
        }

        public RobotStatusUpdate ProcessResponse(string returnValue)
        {
            return new RobotStatusUpdate();
        }
    }
}
