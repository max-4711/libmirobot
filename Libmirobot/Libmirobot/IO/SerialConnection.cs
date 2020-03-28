using Libmirobot.Core;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Timers;

namespace Libmirobot.IO
{
    public class SerialConnection : ISerialConnection, IDisposable
    {
        public event EventHandler<RobotTelegram> TelegramReceived;
        public event EventHandler<RobotTelegram> TelegramSent;

        private SerialPort serialPort = new SerialPort();

        bool isConnecting = false;
        bool isConnected = false;
        bool useQueueingMode = true;
        string lastSentInstructionIdentifier;
        Timer telegramSendTimer = new Timer(50); //Mirobot reportedly struggles with receiving instructions in a higher frequency than 20 Hz: http://discuz.wlkata.com/forum.php?mod=viewthread&tid=8&extra=page%3D1
        Queue<RobotTelegram> OutboundTelegramQueue = new Queue<RobotTelegram>();

        public SerialConnection(string portName, bool useQueueingMode = true)
        {
            serialPort.PortName = portName;
            serialPort.BaudRate = 115200;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            this.useQueueingMode = useQueueingMode;

            if (this.useQueueingMode)
            {
                telegramSendTimer.Elapsed += TelegramSendTimer_Elapsed;
                this.telegramSendTimer.AutoReset = true;
            }
        }

        public void AttachRobot(ISixAxisRobot robot)
        {
            robot.InstructionSent -= Robot_InstructionSent;
            robot.InstructionSent += Robot_InstructionSent;
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
            lastSentInstructionIdentifier = robotTelegram.InstructionIdentifier;
            this.serialPort.Write(robotTelegram.Data);
            this.TelegramSent?.Invoke(this, robotTelegram);
        }

        public void Connect()
        {
            if (!isConnecting)
            {
                isConnecting = true;
                serialPort.Open();
                serialPort.DataReceived -= SerialPort_DataReceived;
                serialPort.DataReceived += SerialPort_DataReceived;
                isConnected = true;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {            
            var responseLine = serialPort.ReadLine(); //https://stackoverflow.com/a/12675073
            this.TelegramReceived?.Invoke(this, new RobotTelegram { InstructionIdentifier = lastSentInstructionIdentifier, Data = responseLine });
        }

        public void Disconnect()
        {
            if (isConnected)
            {
                serialPort.Close();

                isConnected = false;
                isConnecting = false;
            }
        }


        public static IList<string> GetAvailablePortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public void Dispose()
        {
            this.telegramSendTimer.Stop();
            this.serialPort.Close();

            this.telegramSendTimer.Dispose();
            this.serialPort.Dispose();
        }
    }
}
