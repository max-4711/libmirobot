using Libmirobot.Core;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace Libmirobot.IO
{
    public class SerialConnection : ISerialConnection
    {
        public event EventHandler<RobotTelegram> TelegramReceived;

        private SerialPort serialPort = new SerialPort();

        public SerialConnection(string portName)
        {
            serialPort.PortName = portName;
            serialPort.BaudRate = 115200;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;            
        }

        public void AttachRobot(ISixAxisRobot robot)
        {
            robot.InstructionSent -= Robot_InstructionSent;
            robot.InstructionSent += Robot_InstructionSent;
        }

        string lastSentInstructionIdentifier;
        private void Robot_InstructionSent(object sender, RobotTelegram e)
        {
            if (!isConnected)
                return;

            serialPort.Write(e.Data);
            lastSentInstructionIdentifier = e.InstructionIdentifier;
        }

        bool isConnecting = false;
        bool isConnected = false;
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
    }
}
