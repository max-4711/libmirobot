namespace Libmirobot.GCode
{
    /// <summary>
    /// Represents an updated state of the robot
    /// </summary>
    public class RobotStatusUpdate
    {
        /// <summary>
        /// Information, if this update contains new information at all.
        /// </summary>
        public bool HasData => (this.IsIdle.HasValue || this.Axis1Angle.HasValue || this.Axis2Angle.HasValue || this.Axis3Angle.HasValue || this.Axis4Angle.HasValue || this.Axis5Angle.HasValue || this.Axis6Angle.HasValue || this.ExternalSlideRail.HasValue || this.Pwm1.HasValue || this.Pwm2.HasValue || this.XCoordinate.HasValue || this.XRotation.HasValue || this.YCoordinate.HasValue || this.YRotation.HasValue || this.ZCoordinate.HasValue || this.ZRotation.HasValue);

        /// <summary>
        /// True, if the robot is currently idle. False, if the robot is currently in motion.
        /// </summary>
        public bool? IsIdle { get; set; }

        /// <summary>
        /// Current angle of the first axis.
        /// </summary>
        public double? Axis1Angle { get; set; }

        /// <summary>
        /// Current angle of the second axis.
        /// </summary>
        public double? Axis2Angle { get; set; }

        /// <summary>
        /// Current angle of the third axis.
        /// </summary>
        public double? Axis3Angle { get; set; }

        /// <summary>
        /// Current angle of the fourth axis.
        /// </summary>
        public double? Axis4Angle { get; set; }

        /// <summary>
        /// Current angle of the fifth axis.
        /// </summary>
        public double? Axis5Angle { get; set; }

        /// <summary>
        /// Current angle of the sixth axis.
        /// </summary>
        public double? Axis6Angle { get; set; }

        /// <summary>
        /// Current position of the external slide rail (if installed).
        /// </summary>
        public double? ExternalSlideRail { get; set; }

        /// <summary>
        /// X coordinate of the current position.
        /// </summary>
        public double? XCoordinate { get; set; }

        /// <summary>
        /// Y coordinate of the current position.
        /// </summary>
        public double? YCoordinate { get; set; }

        /// <summary>
        /// Z coordinate of the current position.
        /// </summary>
        public double? ZCoordinate { get; set; }

        /// <summary>
        /// Current roll angle (rotation around the x axis).
        /// </summary>
        public double? XRotation { get; set; }

        /// <summary>
        /// Current pitch angle (rotation around the y axis).
        /// </summary>
        public double? YRotation { get; set; }

        /// <summary>
        /// Current yaw angle (rotation around the z axis).
        /// </summary>
        public double? ZRotation { get; set; }

        /// <summary>
        /// Current pwm value of the pneumatic pump (if installed).
        /// </summary>
        public int? Pwm1 { get; set; }

        /// <summary>
        /// Current pwm value of the gripper.
        /// </summary>
        public int? Pwm2 { get; set; }
    }
}
