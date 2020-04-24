using Libmirobot.GCode;

namespace Libmirobot.Core
{
    /// <summary>
    /// Contains positioning parameters for the robot; can be used for angle mode as well as coordinate mode
    /// </summary>
    public class RobotPositionParameter
    {
        /// <summary>
        /// First positioning parameter
        /// </summary>
        public double PositioningParameter1 { get; set; }

        /// <summary>
        /// Second positioning parameter
        /// </summary>
        public double PositioningParameter2 { get; set; }

        /// <summary>
        /// Third positioning parameter
        /// </summary>
        public double PositioningParameter3 { get; set; }

        /// <summary>
        /// Fourth positioning parameter
        /// </summary>
        public double PositioningParameter4 { get; set; }

        /// <summary>
        /// Fifth positioning parameter
        /// </summary>
        public double PositioningParameter5 { get; set; }

        /// <summary>
        /// Sixth positioning parameter
        /// </summary>
        public double PositioningParameter6 { get; set; }

        /// <summary>
        /// Creates a RobotPositionParameter object for the cartesian position within the robot status update
        /// </summary>
        public static RobotPositionParameter? FromRobotStatusUpdateCartesian(RobotStatusUpdate? robotStatusUpdate)
        {
            if (robotStatusUpdate == null)
                return null;

            if (!robotStatusUpdate.XCoordinate.HasValue || !robotStatusUpdate.YCoordinate.HasValue || !robotStatusUpdate.ZCoordinate.HasValue || !robotStatusUpdate.XRotation.HasValue || !robotStatusUpdate.YRotation.HasValue || !robotStatusUpdate.ZRotation.HasValue)
                return null;

            return new RobotPositionParameter
            {
                PositioningParameter1 = robotStatusUpdate.XCoordinate.Value,
                PositioningParameter2 = robotStatusUpdate.YCoordinate.Value,
                PositioningParameter3 = robotStatusUpdate.ZCoordinate.Value,
                PositioningParameter4 = robotStatusUpdate.XRotation.Value,
                PositioningParameter5 = robotStatusUpdate.YRotation.Value,
                PositioningParameter6 = robotStatusUpdate.ZRotation.Value
            };
        }

        /// <summary>
        /// Creates a RobotPositionParameter object for the angle mode position within the robot status update
        /// </summary>
        public static RobotPositionParameter? FromRobotStatusUpdateAngle(RobotStatusUpdate? robotStatusUpdate)
        {
            if (robotStatusUpdate == null)
                return null;

            if (!robotStatusUpdate.Axis1Angle.HasValue || !robotStatusUpdate.Axis2Angle.HasValue || !robotStatusUpdate.Axis3Angle.HasValue || !robotStatusUpdate.Axis4Angle.HasValue || !robotStatusUpdate.Axis5Angle.HasValue || !robotStatusUpdate.Axis6Angle.HasValue)
                return null;

            return new RobotPositionParameter
            {
                PositioningParameter1 = robotStatusUpdate.Axis1Angle.Value,
                PositioningParameter2 = robotStatusUpdate.Axis2Angle.Value,
                PositioningParameter3 = robotStatusUpdate.Axis3Angle.Value,
                PositioningParameter4 = robotStatusUpdate.Axis4Angle.Value,
                PositioningParameter5 = robotStatusUpdate.Axis5Angle.Value,
                PositioningParameter6 = robotStatusUpdate.Axis6Angle.Value
            };
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public static bool operator ==(RobotPositionParameter? a, RobotPositionParameter? b)
        {
            if (a == null && b == null)
                return true;

            if (a == null || b == null)
                return false;

            return a.PositioningParameter1 == b.PositioningParameter1 &&
                a.PositioningParameter2 == b.PositioningParameter2 &&
                a.PositioningParameter3 == b.PositioningParameter3 &&
                a.PositioningParameter4 == b.PositioningParameter6 &&
                a.PositioningParameter5 == b.PositioningParameter4 &&
                a.PositioningParameter6 == b.PositioningParameter6;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public static bool operator !=(RobotPositionParameter? a, RobotPositionParameter? b)
        {
            return !(a == b);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
