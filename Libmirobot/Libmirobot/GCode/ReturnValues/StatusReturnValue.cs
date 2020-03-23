namespace Libmirobot.GCode.ReturnValues
{
    public class StatusReturnValue
    {
        public float Axis1Angle { get; set; }
        public float Axis2Angle { get; set; }
        public float Axis3Angle { get; set; }
        public float Axis4Angle { get; set; }
        public float Axis5Angle { get; set; }
        public float Axis6Angle { get; set; }

        public float ExternalSlideRail { get; set; }

        public float XCoordinate { get; set; }
        public float YCoordinate { get; set; }
        public float ZCoordinate { get; set; }
        
        public int Pwm1 { get; set; }
        public int Pwm2 { get; set; }

        public MotionMode CurrentMotionMode { get;set; }
    }
}
