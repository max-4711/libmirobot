using System;

namespace Libmirobot.Core
{
    /// <summary>
    /// Represents an (updated) state of the robot
    /// </summary>
    public class RobotStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// True, if the robot currently is idle. False, if the robot currently is in motion.
        /// </summary>
        public bool IsIdle { get; set; }

        /// <summary>
        /// Current angle of the first axis.
        /// </summary>
        public double Axis1Angle { get; set; }

        /// <summary>
        /// Current angle of the second axis.
        /// </summary>
        public double Axis2Angle { get; set; }

        /// <summary>
        /// Current angle of the third axis.
        /// </summary>
        public double Axis3Angle { get; set; }

        /// <summary>
        /// Current angle of the fourth axis.
        /// </summary>
        public double Axis4Angle { get; set; }

        /// <summary>
        /// Current angle of the fifth axis.
        /// </summary>
        public double Axis5Angle { get; set; }

        /// <summary>
        /// Current angle of the sixth axis.
        /// </summary>
        public double Axis6Angle { get; set; }
               
        /// <summary>
        /// Current position of the external slide rail (if installed).
        /// </summary>
        public double ExternalSlideRail { get; set; }
               
        /// <summary>
        /// X coordinate of the current position.
        /// </summary>
        public double XCoordinate { get; set; }

        /// <summary>
        /// Y coordinate of the current position.
        /// </summary>
        public double YCoordinate { get; set; }

        /// <summary>
        /// Z coordinate of the current position.
        /// </summary>
        public double ZCoordinate { get; set; }
          
        /// <summary>
        /// Current roll angle (rotation around the x axis).
        /// </summary>
        public double XRotation { get; set; }

        /// <summary>
        /// Current pitch angle (rotation around the y axis).
        /// </summary>
        public double YRotation { get; set; }

        /// <summary>
        /// Current yaw angle (rotation around the z axis).
        /// </summary>
        public double ZRotation { get; set; }

        /// <summary>
        /// Current pwm value of the pneumatic pump (if installed).
        /// </summary>
        public int PneumaticPumpPwm { get; set; }

        /// <summary>
        /// Current pwm value of the gripper.
        /// </summary>
        public int GripperPwm { get; set; }        
    }
}
