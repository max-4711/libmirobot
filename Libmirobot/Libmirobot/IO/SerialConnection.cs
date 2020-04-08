using Libmirobot.Core;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Timers;

namespace Libmirobot.IO
{
    /// <summary>
    /// Represents a serial connection to the hardware.
    /// </summary>
    public class SerialConnection : ISerialConnection
    {
        /// <inheritdoc/>
        public event EventHandler<RobotTelegram>? TelegramReceived;

        /// <inheritdoc/>
        public event EventHandler<RobotTelegram>? TelegramSent;

        private SerialPort serialPort = new SerialPort();

        bool isConnecting = false;
        bool isConnected = false;
        bool useQueueingMode = true;
        string lastSentInstructionIdentifier = string.Empty;
        Timer telegramSendTimer = new Timer(50); //Mirobot reportedly struggles with receiving instructions in a higher frequency than 20 Hz: http://discuz.wlkata.com/forum.php?mod=viewthread&tid=8&extra=page%3D1
        Queue<RobotTelegram> OutboundTelegramQueue = new Queue<RobotTelegram>();

        /// <summary>
        /// Instances a new serial connection using the given port name.
        /// </summary>
        /// <param name="portName">Port name, which shall be used to connect to the hardware.</param>
        /// <param name="useQueueingMode">If true, instructions will be sent to the hardware with a maximum frequency of 20 Hz and therefore queued at first.</param>
        public SerialConnection(string portName, bool useQueueingMode = true)
        {
            this.serialPort.PortName = portName;
            this.serialPort.BaudRate = 115200;
            this.serialPort.DataBits = 8;
            this.serialPort.StopBits = StopBits.One;
            this.useQueueingMode = useQueueingMode;

            if (this.useQueueingMode)
            {
                this.telegramSendTimer.Elapsed += this.TelegramSendTimer_Elapsed;
                this.telegramSendTimer.AutoReset = true;
            }
        }

        /// <inheritdoc/>
        public void AttachRobot(ISixAxisRobot robot)
        {
            robot.InstructionSent -= this.Robot_InstructionSent;
            robot.InstructionSent += this.Robot_InstructionSent;
        }
        
        private void Robot_InstructionSent(object sender, RobotTelegram e)
        {
            if (!this.isConnected)
                return;

            if (this.useQueueingMode)
            {
                this.OutboundTelegramQueue.Enqueue(e);

                if (!this.telegramSendTimer.Enabled)
                {
                    this.telegramSendTimer.Start();
                }
            }
            else
            {
                this.SendTelegram(e);
            }
        }

        private void TelegramSendTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.OutboundTelegramQueue.Count > 0)
            {
                var telegram = this.OutboundTelegramQueue.Dequeue();
                this.SendTelegram(telegram);
            }
            else
            {
                this.telegramSendTimer.Stop();
            }
        }

        private void SendTelegram(RobotTelegram robotTelegram)
        {
            this.lastSentInstructionIdentifier = robotTelegram.InstructionIdentifier;
            this.serialPort.Write(robotTelegram.Data);
            this.TelegramSent?.Invoke(this, robotTelegram);
        }

        /// <inheritdoc/>
        public void Connect()
        {
            if (!this.isConnecting)
            {
                this.isConnecting = true;
                this.serialPort.Open();
                this.serialPort.DataReceived -= this.SerialPort_DataReceived;
                this.serialPort.DataReceived += this.SerialPort_DataReceived;
                this.isConnected = true;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {            
            var responseLine = serialPort.ReadLine(); //https://stackoverflow.com/a/12675073
            this.TelegramReceived?.Invoke(this, new RobotTelegram(lastSentInstructionIdentifier, responseLine));
        }

        /// <inheritdoc/>
        public void Disconnect()
        {
            if (this.isConnected)
            {
                this.serialPort.Close();

                this.isConnected = false;
                this.isConnecting = false;
            }
        }

        /// <summary>
        /// Lists all serial ports, which can be used for communication with the hardware.
        /// </summary>
        /// <returns>List of all serial port names.</returns>
        public static IList<string> GetAvailablePortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        /// <summary>
        /// Disposes the serial connection.
        /// </summary>
        public void Dispose()
        {
            this.telegramSendTimer.Stop();
            this.serialPort.Close();

            this.telegramSendTimer.Dispose();
            this.serialPort.Dispose();
        }
    }
}
