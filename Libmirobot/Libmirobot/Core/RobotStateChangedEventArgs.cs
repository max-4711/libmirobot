using System;

namespace Libmirobot.Core
{
    public class RobotStateChangedEventArgs : EventArgs
    {
        public double Axis1Angle { get; set; }
        public double Axis2Angle { get; set; }
        public double Axis3Angle { get; set; }
        public double Axis4Angle { get; set; }
        public double Axis5Angle { get; set; }
        public double Axis6Angle { get; set; }
               
        public double ExternalSlideRail { get; set; }
               
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public double ZCoordinate { get; set; }
               
        public double XRotation { get; set; }
        public double YRotation { get; set; }
        public double ZRotation { get; set; }

        public int PneumaticPumpPwm { get; set; }
        public int GripperPwm { get; set; }        
    }
}
