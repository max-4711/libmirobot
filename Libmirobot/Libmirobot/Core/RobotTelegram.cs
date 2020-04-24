using System;

namespace Libmirobot.Core
{
    /// <summary>
    /// Represents a g code instruction for the hardware or the response for an instruction from the hardware.
    /// </summary>
    public class RobotTelegram : EventArgs
    {
        /// <summary>
        /// Instances a new robot telegram.
        /// </summary>
        /// <param name="instructionIdentifier">Unique identifier for the instruction type</param>
        /// <param name="data">Instruction as it will be sent to the robot</param>
        public RobotTelegram(string instructionIdentifier, string data)
        {
            this.InstructionIdentifier = instructionIdentifier;
            this.Data = data;
        }

        /// <summary>
        /// G code instruction or hardware response
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Unique identifier of the g code instruction which was used to generate the g code (or which was most likely the initial trigger for the hardware response).
        /// </summary>
        public string InstructionIdentifier { get; set; }

        /// <summary>
        /// Returns, based on the current angle mode target position of the robot, the new angle mode robot target position as it is modified by the telegram
        /// </summary>
        public Func<RobotPositionParameter?, RobotPositionParameter?>? RobotAngleTargetPositionModificator;

        /// <summary>
        /// Returns, based on the current cartesian mode target position of the robot, the new cartesian mode robot target position as it is modified by the telegram
        /// </summary>
        public Func<RobotPositionParameter?, RobotPositionParameter?>? RobotCartesianTargetPositionModificator;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Data} ({this.InstructionIdentifier})";
        }
    }
}