using System;

namespace Libmirobot.Core
{
    /// <summary>
    /// Represents a detected reset of the robot
    /// </summary>
    public class RobotResetEventArgs : EventArgs
    {
        /// <summary>
        /// If true, a homing operation was already requested and thus no further action is required. If false, further action is required.
        /// </summary>
        public bool Handled { get; set; }
    }
}
