namespace Libmirobot.GCode
{
    public class RobotStatusUpdate
    {
        public bool HasData => (this.Axis1Angle.HasValue || this.Axis2Angle.HasValue || this.Axis3Angle.HasValue || this.Axis4Angle.HasValue || this.Axis5Angle.HasValue || this.Axis6Angle.HasValue || this.ExternalSlideRail.HasValue || this.Pwm1.HasValue || this.Pwm2.HasValue || this.XCoordinate.HasValue || this.XRotation.HasValue || this.YCoordinate.HasValue || this.YRotation.HasValue || this.ZCoordinate.HasValue || this.ZRotation.HasValue);

        public double? Axis1Angle { get; set; }
        public double? Axis2Angle { get; set; }
        public double? Axis3Angle { get; set; }
        public double? Axis4Angle { get; set; }
        public double? Axis5Angle { get; set; }
        public double? Axis6Angle { get; set; }
               
        public double? ExternalSlideRail { get; set; }
               
        public double? XCoordinate { get; set; }
        public double? YCoordinate { get; set; }
        public double? ZCoordinate { get; set; }
               
        public double? XRotation { get; set; }
        public double? YRotation { get; set; }
        public double? ZRotation { get; set; }

        public int? Pwm1 { get; set; }
        public int? Pwm2 { get; set; }
    }
}
