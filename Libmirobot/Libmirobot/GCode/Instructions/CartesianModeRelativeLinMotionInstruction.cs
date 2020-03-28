﻿using Libmirobot.GCode.InstructionParameters;

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
            return $"M20 G91 G1 X{inputValue.PositioningParameter1} Y{inputValue.PositioningParameter2} Z{inputValue.PositioningParameter3} A{inputValue.PositioningParameter4} B{inputValue.PositioningParameter5} C{inputValue.PositioningParameter6} F{inputValue.Speed}";
        }

        public RobotStatusUpdate ProcessResponse(string returnValue)
        {
            return new RobotStatusUpdate();
        }
    }
}