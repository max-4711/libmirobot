﻿using Libmirobot.Core;
using Libmirobot.IO;
using System;

namespace Libmirobot
{
    /// <summary>
    /// Capsulation object for a preconfigured robot and serial port, obtained from the RobotConfigurator.
    /// </summary>
    public class PreconfiguredRobot : IDisposable
    {
        internal PreconfiguredRobot(ISixAxisRobot robot, ISerialConnection telegramPort)
        {
            this.Robot = robot;
            this.TelegramPort = telegramPort;
        }

        /// <summary>
        /// Six axis robot, attached to the serial port.
        /// </summary>
        public ISixAxisRobot Robot { get; }

        /// <summary>
        /// Serial port used for communication with the hardware, attached to the robot.
        /// </summary>
        public ISerialConnection TelegramPort { get; }

        /// <summary>
        /// Event being fired, when a new message from the hardware is received via the serial port.
        /// </summary>
        public event EventHandler<RobotTelegram> TelegramReceived
        {
            add { this.TelegramPort.TelegramReceived += value; }
            remove { this.TelegramPort.TelegramReceived -= value; }
        }

        /// <summary>
        /// Event being fired, when a new message to the hardware is being sent via the serial port.
        /// </summary>
        public event EventHandler<RobotTelegram> TelegramSent
        {
            add { this.TelegramPort.TelegramSent += value; }
            remove { this.TelegramPort.TelegramSent -= value; }
        }

        /// <summary>
        /// Event being fired, when the robot component detects a status update.
        /// </summary>
        /// <remarks>Will fire when the hardware's response for the status request arrives. Status request can be triggered via UpdateCurrentPosition() method or by setting AutoSendStatusUpdateRequests property to true.</remarks>
        public event EventHandler<RobotStateChangedEventArgs> RobotStateChanged
        {
            add { this.Robot.RobotStateChanged += value; }
            remove { this.Robot.RobotStateChanged -= value; }
        }

        /// <summary>
        /// Event being fired, when the hardware reports an incident, such as an alarm or a triggered limit.
        /// </summary>
        /// <remarks>Manual user interaction may be required, depending on the error.</remarks>
        public event EventHandler<RobotErrorEventArgs> RobotErrorOccured
        {
            add { this.Robot.RobotErrorOccurred += value; }
            remove { this.Robot.RobotErrorOccurred -= value; }
        }

        /// <summary>
        /// Event being fired, when the hardware reports a reset.
        /// </summary>
        /// <remarks>Re-performing the homing operation may be necessary (but can be handled automatically).</remarks>
        public event EventHandler<RobotResetEventArgs> RobotResetOccurred
        {
            add { this.Robot.RobotResetOccurred += value; }
            remove { this.Robot.RobotResetOccurred -= value; }
        }

        /// <summary>
        /// Disposes the serial port and thereby terminates the connection to the hardware.
        /// </summary>
        public void Dispose()
        {
            this.TelegramPort.Dispose();
            this.Robot.Dispose();
        }
    }
}
