using System;

namespace Libmirobot.Core
{
    public class InstructionSentEventArgs : EventArgs
    {
        public string Instruction { get; set; }

        public string InstructionIdentifier { get; set; }
    }
}