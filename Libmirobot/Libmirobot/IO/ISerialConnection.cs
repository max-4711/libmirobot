using Libmirobot.Core;
using System;

namespace Libmirobot.IO
{
    public interface ISerialConnection
    {
        event EventHandler<RobotTelegram> TelegramReceived;

        void AttachRobot(ISixAxisRobot robot);

        void Connect();

        void Disconnect();
    }
}
