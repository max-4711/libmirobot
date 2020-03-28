using Libmirobot.Core;
using Libmirobot.IO;
using System;

namespace Libmirobot
{
    public class RobotFactory
    {
        //Not a real factory yet; maybe adding that later
        public static PreconfiguredRobot PreconfigureRobot(string comPortName)
        {
            var robot = SixAxisMirobot.CreateNew();
            var telegramPort = new SerialConnection(comPortName);

            robot.AttachConnection(telegramPort);
            telegramPort.AttachRobot(robot);

            return new PreconfiguredRobot(robot, telegramPort);
        }
    }

    public class PreconfiguredRobot
    {
        internal PreconfiguredRobot(ISixAxisRobot robot, ISerialConnection telegramPort)
        {
            this.Robot = robot;
            this.TelegramPort = telegramPort;
        }

        public ISixAxisRobot Robot { get; }
        public ISerialConnection TelegramPort { get; }

        public event EventHandler<RobotTelegram> InboundTelegram
        {
            add { this.TelegramPort.TelegramReceived += value; }
            remove { this.TelegramPort.TelegramReceived -= value; }
        }

        public event EventHandler<RobotTelegram> OutboundTelegram
        {
            add { this.Robot.InstructionSent += value; }
            remove { this.Robot.InstructionSent -= value; }
        }

        public event EventHandler<RobotStateChangedEventArgs> RobotStateChanged
        {
            add { this.RobotStateChanged += value; }
            remove { this.RobotStateChanged -= value; }
        }
    }
}
