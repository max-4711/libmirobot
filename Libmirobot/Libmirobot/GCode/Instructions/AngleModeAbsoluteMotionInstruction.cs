using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to move to the specified absolute position in angle mode.
    /// </summary>
    /// <remarks>Instruction no. 5 as specified in protocol specification.</remarks>
    public class AngleModeAbsoluteMotionInstruction : IGCodeInstruction<MotionInstructionParameter>
    {
        public string UniqueIdentifier => "MOTION_ANGLE_ABSOLUTE";

        public string GenerateGCode(MotionInstructionParameter inputValue)
        {
            return $"M21 G90 X{inputValue.PositioningParameter1} Y{inputValue.PositioningParameter2} Z{inputValue.PositioningParameter3} A{inputValue.PositioningParameter4} B{inputValue.PositioningParameter5} C{inputValue.PositioningParameter6} F{inputValue.Speed}";
        }
    }
}
