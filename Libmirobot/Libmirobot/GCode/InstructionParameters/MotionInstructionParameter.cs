namespace Libmirobot.GCode.InstructionParameters
{
    public class MotionInstructionParameter
    {
        /// <summary>
        /// Specifies a value for the x axis - depending on the instruction either an increment or a coordinate.
        /// </summary>
        public float XAxisValue { get; set; }

        /// <summary>
        /// Specifies a value for the y axis - depending on the instruction either an increment or a coordinate.
        /// </summary>
        public float YAxisValue { get; set; }

        /// <summary>
        /// Specifies a value for the z axis - depending on the instruction either an increment or a coordinate.
        /// </summary>
        public float ZAxisValue { get; set; }

        /// <summary>
        /// Specifies a value for the roll angle - depending on the instruction either an increment or an absolute angle
        /// </summary>
        public float XRotationValue { get; set; }

        /// <summary>
        /// Specifies a value for the pitch angle - depending on the instruction either an increment or an absolute angle
        /// </summary>
        public float YRotationValue { get; set; }

        /// <summary>
        /// Specifies a value for the yaw angle - depending on the instruction either an increment or an absolute angle
        /// </summary>
        public float ZRotationValue { get; set; }

        /// <summary>
        /// Desired speed of the motion
        /// </summary>
        /// <remarks>Specified in mm/min.</remarks>
        public int Speed { get; set; }
    }
}
