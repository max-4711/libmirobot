using Libmirobot.GCode;

namespace Libmirobot.Core
{
    public class SixAxisMirobot : ISixAxisRobot
    {
        public void HomeAxes(HomingMode homingMode)
        {
            throw new System.NotImplementedException();
        }

        public void IncrementAxes(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed, MovementMode movementMode)
        {
            throw new System.NotImplementedException();
        }

        public void IncrementCartesian(float xCoordinateIncrement, float yCoordinateIncrement, float zCoordinateIncrement, float xRotationIncrement, float yRotationIncrement, float zRotationIncrement, int speed, MovementMode movementMode)
        {
            throw new System.NotImplementedException();
        }

        public void MoveAxesTo(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed, MovementMode movementMode)
        {
            throw new System.NotImplementedException();
        }

        public void MoveToCartesian(float xCoordinate, float yCoordinate, float zCoordinate, float xRotation, float yRotation, float zRotation, int speed, MovementMode movementMode)
        {
            throw new System.NotImplementedException();
        }

        public void SetAirPumpPower(int pwm)
        {
            throw new System.NotImplementedException();
        }

        public void SetAxesHardLimit(bool on)
        {
            throw new System.NotImplementedException();
        }

        public void SetAxesSoftLimit(bool on)
        {
            throw new System.NotImplementedException();
        }

        public void SetGripperAperture(int pwm)
        {
            throw new System.NotImplementedException();
        }

        public void UnlockAxes()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCurrentPosition()
        {
            throw new System.NotImplementedException();
        }
    }
}
