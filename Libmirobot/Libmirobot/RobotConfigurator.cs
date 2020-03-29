using Libmirobot.Core;
using Libmirobot.IO;
using System.Collections.Generic;

namespace Libmirobot
{
    /// <summary>
    /// Helper to simplify the act of setting up a robot and serial port.
    /// </summary>
    public class RobotConfigurator
    {
        //Not a real factory yet; maybe adding that later
        
        /// <summary>
        /// Initializes a six axis robot, a serial port and wires them together.
        /// </summary>
        /// <param name="comPortName">Name of the serial port used for communication with the hardware.</param>
        /// <returns>Preconfigured robot, with robot and serial port ready for use.</returns>
        public static PreconfiguredRobot PreconfigureRobot(string comPortName)
        {
            var robot = SixAxisMirobot.CreateNew();
            var telegramPort = new SerialConnection(comPortName, true);

            robot.AttachConnection(telegramPort);
            telegramPort.AttachRobot(robot);

            telegramPort.Connect();

            return new PreconfiguredRobot(robot, telegramPort);
        }

        /// <summary>
        /// Lists all serial ports, which can be used for communication with the hardware.
        /// </summary>
        /// <returns>List of all serial port names.</returns>
        public static IList<string> GetAvailableComports()
        {
            return SerialConnection.GetAvailablePortNames();
        }
    }
}
