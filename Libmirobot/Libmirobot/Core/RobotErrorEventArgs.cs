using System;

namespace Libmirobot.Core
{
    /// <summary>
    /// Represents an incident with the robot. Manual user interaction may be necessary.
    /// </summary>
    public class RobotErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initiates a new RobotEventErrorEventArgs instance.
        /// </summary>
        /// <param name="message">Message, as received from the robot</param>
        public RobotErrorEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Message, as it was received from the robot
        /// </summary>
        public string Message { get; }
    }
}
