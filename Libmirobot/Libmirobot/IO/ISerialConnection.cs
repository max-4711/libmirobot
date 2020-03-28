using Libmirobot.Core;
using System;

namespace Libmirobot.IO
{
    public interface ISerialConnection
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        void Attach(ISixAxisRobot robot);

        void Connect();
    }
}
