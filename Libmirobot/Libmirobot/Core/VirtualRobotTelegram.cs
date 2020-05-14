using System;

namespace Libmirobot.Core
{
    /// <summary>
    /// Represents a virtual telegram, which is something handled like a telegram, but not actually being sent to the robot (performs some setting changes for example)
    /// </summary>
    public class VirtualRobotTelegram : RobotTelegram
    {
        private Action telegramAction;

        /// <summary>
        /// Instances a new virtual robot telegram.
        /// </summary>
        /// <param name="instructionIdentifier">Unique identifier for the instruction type</param>
        /// <param name="telegramAction">Action, which is being executed when a regular telegram would be sent to the hardware</param>
        public VirtualRobotTelegram(string instructionIdentifier, Action telegramAction) : base(instructionIdentifier, "NO-DATA--VIRTUAL-TELEGRAM")
        {
            this.telegramAction = telegramAction;
        }

        /// <summary>
        /// Executes the telegram action
        /// </summary>
        public void Execute()
        {
            this.telegramAction();
        }
    }
}
