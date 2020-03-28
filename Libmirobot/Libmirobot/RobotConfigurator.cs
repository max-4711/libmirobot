using Libmirobot.Core;
using Libmirobot.IO;
using System;
using System.Collections.Generic;

namespace Libmirobot
{
    public class RobotConfigurator
    {
        //Not a real factory yet; maybe adding that later
        public static PreconfiguredRobot PreconfigureRobot(string comPortName)
        {
            var robot = SixAxisMirobot.CreateNew();
            var telegramPort = new SerialConnection(comPortName, true);

            robot.AttachConnection(telegramPort);
            telegramPort.AttachRobot(robot);

            telegramPort.Connect();

            return new PreconfiguredRobot(robot, telegramPort);
        }

        public static IList<string> GetAvailableComports()
        {
            return SerialConnection.GetAvailablePortNames();
        }
    }

    public class PreconfiguredRobot : IDisposable
    {
        internal PreconfiguredRobot(ISixAxisRobot robot, ISerialConnection telegramPort)
        {
            this.Robot = robot;
            this.TelegramPort = telegramPort;
        }

        public ISixAxisRobot Robot { get; }
        public ISerialConnection TelegramPort { get; }

        public event EventHandler<RobotTelegram> TelegramReceived
        {
            add { this.TelegramPort.TelegramReceived += value; }
            remove { this.TelegramPort.TelegramReceived -= value; }
        }

        public event EventHandler<RobotTelegram> TelegramSent
        {
            add { this.TelegramPort.TelegramSent += value; }
            remove { this.TelegramPort.TelegramSent -= value; }
        }

        public event EventHandler<RobotStateChangedEventArgs> RobotStateChanged
        {
            add { this.Robot.RobotStateChanged += value; }
            remove { this.Robot.RobotStateChanged -= value; }
        }

        public void Dispose()
        {
            this.TelegramPort.Dispose();
        }
    }
}
