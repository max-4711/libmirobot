﻿using Libmirobot.GCode.InstructionParameters;

namespace Libmirobot.GCode.Instructions
{
    /// <summary>
    /// Instructs the robot to open or close the gripper.
    /// </summary>
    /// <remarks>Instruction no. 14 as specified in protocol specification.</remarks>
    public class SwitchGripperInstruction : IGCodeInstruction<IntegerInstructionParameter>
    {
        /// <inheritdoc/>
        public string UniqueIdentifier => "SWITCH_GRIPPER";

        /// <inheritdoc/>
        public bool IsMotionInstruction => true;

        /// <inheritdoc/>
        public string GenerateGCode(IntegerInstructionParameter inputValue)
        {
            if (inputValue.Parameter > 65)
                inputValue.Parameter = 65;

            if (inputValue.Parameter < 40)
                inputValue.Parameter = 40;

            return $"M4E{inputValue.Parameter}";
        }

#if NETSTANDARD2_0
        /// <inheritdoc />
        public bool CanProcessResponse(string response)
        {
            return false;
        }

        /// <inheritdoc />
        public RobotStatusUpdate ProcessResponse(string returnValue)
        {
            return new RobotStatusUpdate();
        }
#endif
    }
}
