using Libmirobot.IO;
using System;

namespace Libmirobot.Core
{
    /// <summary>
    /// Represents a robot with 6(+1) axes.
    /// </summary>
    public interface ISixAxisRobot : IDisposable
    {
        /// <summary>
        /// Event being fired, when the robot issues a new command, which now has to be transferred to the hardware.
        /// </summary>
        event EventHandler<RobotTelegram> InstructionSent;

        /// <summary>
        /// Event being fired, when the robot received a status update from the hardware.
        /// </summary>
        event EventHandler<RobotStateChangedEventArgs> RobotStateChanged;

        /// <summary>
        /// Event being fired, when the hardware reports an incident. Manual user interaction may be required.
        /// </summary>
        event EventHandler<RobotErrorEventArgs> RobotErrorOccurred;

        /// <summary>
        /// Event being fired, when the hardware reports being resetted. Re-performing the homing operation may be necessary.
        /// </summary>
        event EventHandler<RobotResetEventArgs> RobotResetOccurred;

        /// <summary>
        /// Configures the robot to receive messages from the hardware via the serial connection.
        /// </summary>
        /// <param name="serialConnection">Serial connection which the robot will use to receive commands from</param>
        void AttachConnection(ISerialConnection serialConnection);

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
        /// <param name="speed">Desired maximum speed for movement execution. Specified in mm/min. Defaults to 2000.</param>
        /// <param name="movementMode">Specifies, if the axes coordinate their motion with each other. Defaults to MovementMode.Linear.</param>
        void MoveToCartesian(float xCoordinate, float yCoordinate, float zCoordinate, float xRotation, float yRotation, float zRotation, int speed = 2000, MovementMode movementMode = MovementMode.Linear);

        /// <summary>
        /// Instructs the robot to increment (or decrement, if negative numbers provided) his current location and angles by the specified values using the specified movement mode.
        /// </summary>
        /// <param name="xCoordinateIncrement">Increment/Decrement for the x axis (cartesian)</param>
        /// <param name="yCoordinateIncrement">Increment/Decrement for the y axis (cartesian)</param>
        /// <param name="zCoordinateIncrement">Increment/Decrement for the z axis (cartesian)</param>
        /// <param name="xRotationIncrement">Increment/Decrement for the roll angle (rotation around the x axis)</param>
        /// <param name="yRotationIncrement">Increment/Decrement for the pitch angle (rotation around the y axis)</param>
        /// <param name="zRotationIncrement">Increment/Decrement for the yaw angle (rotation around the z axis)</param>
        /// <param name="speed">Desired maximum speed for movement execution. Specified in mm/min. Defaults to 2000.</param>
        /// <param name="movementMode">Specifies, if the axes coordinate their motion with each other. Defaults to MovementMode.Linear.</param>
        void IncrementCartesian(float xCoordinateIncrement, float yCoordinateIncrement, float zCoordinateIncrement, float xRotationIncrement, float yRotationIncrement, float zRotationIncrement, int speed = 2000, MovementMode movementMode = MovementMode.Linear);

        /// <summary>
        /// Instructs the robot to turn each axis to the corresponding angle using the specified movement mode.
        /// </summary>
        /// <param name="axis1">Target angle for axis 1</param>
        /// <param name="axis2">Target angle for axis 2</param>
        /// <param name="axis3">Target angle for axis 3</param>
        /// <param name="axis4">Target angle for axis 4</param>
        /// <param name="axis5">Target angle for axis 5</param>
        /// <param name="axis6">Target angle for axis 6</param>
        /// <param name="speed">Desired maximum soeed for movement execution. Specified in mm/min. Defaults to 2000.</param>
        void MoveAxesTo(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed = 2000);

        /// <summary>
        /// Instructs the robot to increment (or decrement, if negative numbers provided) his axis angles by the specified values using the specified movement mode.
        /// </summary>
        /// <param name="axis1">Increment/Decrement for axis 1</param>
        /// <param name="axis2">Increment/Decrement for axis 2</param>
        /// <param name="axis3">Increment/Decrement for axis 3</param>
        /// <param name="axis4">Increment/Decrement for axis 4</param>
        /// <param name="axis5">Increment/Decrement for axis 5</param>
        /// <param name="axis6">Increment/Decrement for axis 6</param>
        /// <param name="speed">Desired maximum soeed for movement execution. Specified in mm/min. Defaults to 2000.</param>
        void IncrementAxes(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed = 2000);

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

        /// <summary>
        /// Changes the status of the auto homing setting.
        /// </summary>
        /// <param name="enabled">If true, auto homing will be enabled, else it will be disabled</param>
        /// <param name="delayUntilCommandCompletion">If true, the setting change takes action after all instructions previously sent to the robot were completed. Else it will take action immediately. Defaults to true.</param>
        /// <remarks>Auto homing will only work, if the robot is set up to delay instruction sending until the previous motion instruction has been completed ('delayInstructionUntilPreviousInstructionCompleted' = true at robot setup)</remarks>
        void SetAutoHomingStatus(bool enabled, bool delayUntilCommandCompletion = true);

        /// <summary>
        /// Clears the command queue and thus stops all yet unsent instructions from being sent to the hardware.
        /// </summary>
        void ClearCommandQueue();

        /// <summary>
        /// Will reset all internal status information preventing the robot from sending new instructions after an error occurred. Use only for error recovery. 
        /// </summary>
        void Reset();
    }
}
