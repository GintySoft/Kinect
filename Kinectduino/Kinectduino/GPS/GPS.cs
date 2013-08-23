using System;
using System.IO.Ports;
using System.Collections;
using System.Text;
using System.Threading;
using Microsoft.SPOT;

namespace GintySoft.Kinectduino
{
    public class GPS : IDisposable
    {
        public event GPSPositionDelegate NewGPSLocation;
        public delegate void GPSPositionDelegate(string lat, string lon);
        
        private SerialPort SerialPort { get; set; }
        private NMEAParser Parser { get; set; }
        private Thread ReaderThread { get; set; }
        public GPS(string port)
        {
            ThreadStart ts = new ThreadStart(this.SerialPortRead);
            this.ReaderThread = new Thread(ts);
            this.Parser = new NMEAParser();
            this.Parser.PositionReceived += new NMEAParser.PositionReceivedEventHandler(Parser_PositionReceived);
            this.SerialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            this.SerialPort.ReadTimeout = 1000;
            this.SerialPort.WriteTimeout = 1000;
            this.SerialPort.Open();
            this.ReaderThread.Start();
        }

        private void Parser_PositionReceived(string latitude, string longitude)
        {
            if (this.NewGPSLocation != null)
            {
                this.NewGPSLocation(latitude, longitude);
            }
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
           
        }
        private void SerialPortRead()
        {
            try
            {
                ArrayList currentMessage = new ArrayList();
                while (this.SerialPort.IsOpen)
                {
                    int bytesToRead = this.SerialPort.BytesToRead;
                    if (bytesToRead <= 0)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        byte[] bytes = readBytes(bytesToRead);
                        foreach (byte b in bytes)
                        {
                            char c = (char)b;
                            if (c == '\n')
                            {
                                byte[] messageBytes = (byte[])currentMessage.ToArray(typeof(byte));
                                char[] chars = Encoding.UTF8.GetChars(messageBytes);
                                string message = new string(chars);
                                if (!this.Parser.Parse(message))
                                {
                                    Debug.Print("Could not parse GPS message: " + message);
                                }
                                currentMessage.Clear();
                            }
                            else
                            {
                                currentMessage.Add(b);
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                Debug.Print("Error in gps serial port reader thread: " + ex.Message);
            }
        }

        private byte[] readBytes(int totalCount)
        {
            int cnt = 0;
            int amtRead = 0;
            byte[] bytes = new byte[totalCount];
            while (amtRead < totalCount)
            {
                int actualRead = this.SerialPort.Read(bytes, amtRead, totalCount - amtRead);
                amtRead += actualRead;
                cnt++;
                if (cnt > 10)
                {
                    throw new Exception("Could not read requested amount!");
                }
            }
            return bytes;
        }
        public void Dispose()
        {
            if (this.SerialPort.IsOpen)
            {
                this.SerialPort.Close();
            }
            this.SerialPort.Dispose();
        }
    }
}
