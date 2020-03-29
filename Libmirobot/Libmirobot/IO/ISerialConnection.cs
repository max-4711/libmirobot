using Libmirobot.Core;
using System;

namespace Libmirobot.IO
{
    /// <summary>
    /// Represents a serial connection to the hardware.
    /// </summary>
    public interface ISerialConnection : IDisposable
    {
        /// <summary>
        /// Event being fired, when new messages from the hardware are being received.
        /// </summary>
        event EventHandler<RobotTelegram> TelegramReceived;

        /// <summary>
        /// Event being fired, when new messages to the hardware are being sent.
        /// </summary>
        event EventHandler<RobotTelegram> TelegramSent;

        /// <summary>
        /// Configures the serial connection to listen for new messages from the robot and send them to the hardware
        /// </summary>
        /// <param name="robot">Robot, whose messages shall be sent to the hardware</param>
        void AttachRobot(ISixAxisRobot robot);

        /// <summary>
        /// Establishes the connection to the hardware.
        /// </summary>
        void Connect();

        /// <summary>
        /// Tears down the connection to the hardware.
        /// </summary>
        void Disconnect();
    }
}
