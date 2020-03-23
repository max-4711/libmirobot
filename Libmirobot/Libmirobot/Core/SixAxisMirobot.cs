namespace Libmirobot.Core
{
    public class SixAxisMirobot : ISixAxisRobot
    {
        public float AxisX { get; private set; }

        public float RotationX { get; private set; }

        public float AxisY { get; private set; }

        public float RotationY { get; private set; }

        public float AxisZ { get; private set; }

        public float RotationZ { get; private set; }

        internal SixAxisMirobot()
        {

        }
    }
}
