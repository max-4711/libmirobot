namespace Libmirobot.Core
{
    public interface ISixAxisRobot
    {
        /// <summary>
        /// Current x coordinate
        /// </summary>
        float AxisX { get; }
        
        /// <summary>
        /// Current roll angle (degrees)
        /// </summary>
        float RotationX { get; }
        
        /// <summary>
        /// Current y coordinate
        /// </summary>
        float AxisY { get; }
        
        /// <summary>
        /// Current Pitch angle (degrees)
        /// </summary>
        float RotationY { get; }
        
        /// <summary>
        /// Current z coordinate
        /// </summary>
        float AxisZ { get; }
        
        /// <summary>
        /// Current yaw angle (degrees)
        /// </summary>
        float RotationZ { get; }
    }
}
