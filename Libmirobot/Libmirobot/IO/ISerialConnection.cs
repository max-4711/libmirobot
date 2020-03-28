using Libmirobot.Core;
using System;

namespace Libmirobot.IO
{
    public interface ISerialConnection : IDisposable
    {
        event EventHandler<RobotTelegram> TelegramReceived;

        event EventHandler<RobotTelegram> TelegramSent;

        void AttachRobot(ISixAxisRobot robot);

        void Connect();

        void Disconnect();
    }
}
