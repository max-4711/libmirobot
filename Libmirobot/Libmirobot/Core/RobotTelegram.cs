using System;

namespace Libmirobot.Core
{
    public class RobotTelegram : EventArgs
    {
        public string Data { get; set; }

        public string InstructionIdentifier { get; set; }
    }
}