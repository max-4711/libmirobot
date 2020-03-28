using System;

namespace Libmirobot.IO
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string InstructionIdentifier { get; set; }

        public string ReceivedMessage { get; set; }
    }
}