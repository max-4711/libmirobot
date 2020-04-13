namespace Libmirobot
{
    /// <summary>
    /// Specifies the type of motion, which shall be used when changing the robot end effectors position.
    /// </summary>
    public enum MovementMode
    {
        /// <summary>
        /// All axes will move independently from each other; so they may not finish their movement at the same time.
        /// </summary>
        /// <remarks>Also known as 'fast mode' from the specification.</remarks>
        PointToPoint = 0,

        /// <summary>
        /// Motion is being coordinated between the axes, so that they will finish the target position at the same time and the movement describes a straight line.
        /// </summary>
        Linear = 1
    }
}
