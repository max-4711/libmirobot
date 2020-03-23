namespace Libmirobot.GCode.InstructionParameters
{
    public class MotionInstructionParameter
    {
        public float XCoordinateValue { get; set; }
        public float YCoordinateValue { get; set; }
        public float ZCoordinateValue { get; set; }

        public float RollAngleValue { get; set; }
        public float PitchAngleValue { get; set; }
        public float YawAngleValue { get; set; }

        /// <summary>
        /// Desired speed of the motion
        /// </summary>
        /// <remarks>Specified in mm/min.</remarks>
        public int Speed { get; set; }
    }
}
