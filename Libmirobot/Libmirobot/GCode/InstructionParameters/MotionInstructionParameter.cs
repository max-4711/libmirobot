namespace Libmirobot.GCode.InstructionParameters
{
    public class MotionInstructionParameter
    {
        /// <summary>
        /// First parameter to specify the target position or increment of the motion.
        /// </summary>
        /// <remarks>Typically used for the x axis or axis 1.</remarks>
        public float PositioningParameter1 { get; set; }

        /// <summary>
        /// Second parameter to specify the target position or increment of the motion.
        /// </summary>
        /// <remarks>Typically used for the y axis or axis 2.</remarks>
        public float PositioningParameter2 { get; set; }

        /// <summary>
        /// Third parameter to specify the target position or increment of the motion.
        /// </summary>
        /// <remarks>Typically used for the z axis or axis 3.</remarks>
        public float PositioningParameter3 { get; set; }

        /// <summary>
        /// Fourth parameter to specify the target position or increment of the motion.
        /// </summary>
        /// <remarks>Typically used for the roll angle (rotation around x axis) or axis 4.</remarks>
        public float PositioningParameter4 { get; set; }

        /// <summary>
        /// Fifth parameter to specify the target position or increment of the motion.
        /// </summary>
        /// <remarks>Typically used for the pitch angle (rotation around y axis) or axis 5.</remarks>
        public float PositioningParameter5 { get; set; }

        /// <summary>
        /// Sixth parameter to specify the target position or increment of the motion.
        /// </summary>
        /// <remarks>Typically used for the yaw angle (rotation around z axis) or axis 6.</remarks>
        public float PositioningParameter6 { get; set; }

        /// <summary>
        /// Desired speed of the motion
        /// </summary>
        /// <remarks>Specified in mm/min.</remarks>
        public int Speed { get; set; }
    }
}
