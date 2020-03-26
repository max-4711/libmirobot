namespace Libmirobot.GCode
{
    public enum MovementMode
    {
        /// <summary>
        /// All axes will move independently from each other; so they may not finish their movement at the same time.
        /// </summary>
        PointToPoint = 0,

        /// <summary>
        /// Motion is being coordinated between the axes, so that they will finish the target position at the same time and the movement describes a straight line.
        /// </summary>
        Linear = 1
    }
}
