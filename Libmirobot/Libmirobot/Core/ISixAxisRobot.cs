using Libmirobot.GCode;

namespace Libmirobot.Core
{
    public interface ISixAxisRobot
    {
        /// <summary>
        /// Instructs the robot to report its current position.
        /// </summary>
        void UpdateCurrentPosition();

        /// <summary>
        ///  Instructs the end effector of the robot to move to the specified cartesian coordinates and take on the provided angles using the specified movement mode.
        /// </summary>
        /// <param name="xCoordinate">Target x coordinate (cartesian)</param>
        /// <param name="yCoordinate">Target y coordinate (cartesian)</param>
        /// <param name="zCoordinate">Target z coordinate (cartesian)</param>
        /// <param name="xRotation">Target roll angle (rotation around the x axis)</param>
        /// <param name="yRotation">Target pitch angle (rotation around the y axis)</param>
        /// <param name="zRotation">Target yaw angle (rotation around the z axis)</param>
        /// <param name="speed">Desired maximum speed for movement execution. Specified in mm/min</param>
        /// <param name="movementMode">Specifies, if the axes coordinate their motion with each other</param>
        void MoveToCartesian(float xCoordinate, float yCoordinate, float zCoordinate, float xRotation, float yRotation, float zRotation, int speed, MovementMode movementMode);

        /// <summary>
        /// Instructs the robot to increment (or decrement, if negative numbers provided) his current location and angles by the specified values using the specified movement mode.
        /// </summary>
        /// <param name="xCoordinateIncrement">Increment/Decrement for the x axis (cartesian)</param>
        /// <param name="yCoordinateIncrement">Increment/Decrement for the y axis (cartesian)</param>
        /// <param name="zCoordinateIncrement">Increment/Decrement for the z axis (cartesian)</param>
        /// <param name="xRotationIncrement">Increment/Decrement for the roll angle (rotation around the x axis)</param>
        /// <param name="yRotationIncrement">Increment/Decrement for the pitch angle (rotation around the y axis)</param>
        /// <param name="zRotationIncrement">Increment/Decrement for the yaw angle (rotation around the z axis)</param>
        /// <param name="speed">Desired maximum speed for movement execution. Specified in mm/min</param>
        /// <param name="movementMode">Specifies, if the axes coordinate their motion with each other</param>
        void IncrementCartesian(float xCoordinateIncrement, float yCoordinateIncrement, float zCoordinateIncrement, float xRotationIncrement, float yRotationIncrement, float zRotationIncrement, int speed, MovementMode movementMode);

        /// <summary>
        /// Instructs the robot to turn each axis to the corresponding angle using the specified movement mode.
        /// </summary>
        /// <param name="axis1">Target angle for axis 1</param>
        /// <param name="axis2">Target angle for axis 2</param>
        /// <param name="axis3">Target angle for axis 3</param>
        /// <param name="axis4">Target angle for axis 4</param>
        /// <param name="axis5">Target angle for axis 5</param>
        /// <param name="axis6">Target angle for axis 6</param>
        /// <param name="speed">Desired maximum soeed for movement execution. Specified in mm/min</param>
        /// <param name="movementMode">Specifiecs, if the axes coordinate their motion with each other</param>
        void MoveAxesTo(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed, MovementMode movementMode);

        /// <summary>
        /// Instructs the robot to increment (or decrement, if negative numbers provided) his axis angles by the specified values using the specified movement mode.
        /// </summary>
        /// <param name="axis1">Increment/Decrement for axis 1</param>
        /// <param name="axis2">Increment/Decrement for axis 2</param>
        /// <param name="axis3">Increment/Decrement for axis 3</param>
        /// <param name="axis4">Increment/Decrement for axis 4</param>
        /// <param name="axis5">Increment/Decrement for axis 5</param>
        /// <param name="axis6">Increment/Decrement for axis 6</param>
        /// <param name="speed">Desired maximum soeed for movement execution. Specified in mm/min</param>
        /// <param name="movementMode">Specifiecs, if the axes coordinate their motion with each other</param>
        void IncrementAxes(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed, MovementMode movementMode);

        /// <summary>
        /// Instructs the robot to unlock all axes directly, even if no homing operation was executed.
        /// </summary>
        /// <remarks>It is strongly recommend to perform a homing operation instead of this command to unlock the axes in order to prevent mechanical errors.</remarks>
        void UnlockAxes();

        /// <summary>
        /// Instructs the robot to enable or disable the soft limit function, which will limit the arm movement if the soft kickout range is reached. Applies to all axes.
        /// </summary>
        /// <param name="on">Specifies, whether the soft limit function should be on or off.</param>
        /// <remarks>Is is strongly recommended to leave the soft limit function at their default value (enabled).</remarks>
        void SetAxesSoftLimit(bool on);

        /// <summary>
        /// Instructs the robot to enable or disable the hard limit function, which will stop the shaft movement if the travel switches for the hard limit positions are toggled. Applies to the axes 1, 2 and 3.
        /// </summary>
        /// <param name="on">Specifies, whether the hard limit function should be on or off.</param>
        /// <remarks>It is strongly recommended to leave the hard limit function at their default value (enabled).</remarks>
        void SetAxesHardLimit(bool on);

        /// <summary>
        /// Instructs the robot to perform a homing operation.
        /// </summary>
        /// <param name="homingMode">Specifies, which homing mode will be used for the motion</param>
        void HomeAxes(HomingMode homingMode);

        /// <summary>
        /// Controls the duty cycle of the pneumatic air pump attached to the robot.
        /// </summary>
        /// <param name="pwm">Min 0 (low power), max 1000 (high power)</param>
        void SetAirPumpPower(int pwm);

        /// <summary>
        /// Controls the duty cycle of the gripper
        /// </summary>
        /// <param name="pwm">Min (closed) 40, max (open) 65</param>
        void SetGripperAperture(int pwm);
    }
}
