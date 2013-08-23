using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace SerialPortCommunicator
{
    class Program
    {
        public static SerialPort senderPort;
        public static SerialPort receiverPort;

        static void Main(string[] args)
        {
            receiverPort = new SerialPort();
            receiverPort.BaudRate = 9600;
            receiverPort.PortName = "COM18";
            receiverPort.Open();

            senderPort = new SerialPort();
            senderPort.BaudRate = 4800;
            senderPort.PortName = "COM19";
            senderPort.Open();

            while (true)
            {
                int val = senderPort.ReadByte();
                byte b = (byte)val;
                byte[] bytes = new byte[1];
                bytes[0] = b;   
                receiverPort.Write(bytes, 0, 1);
                Console.Write(bytes[0].ToString());
            }
        }
    }
}
