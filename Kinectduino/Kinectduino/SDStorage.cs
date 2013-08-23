using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace GintySoft.Kinectduino
{
    public class SDStorage
    {
        private DirectoryInfo Root { get; set; }
        private InterruptPort sdCardStatusPort { get; set; }

        public event Globals.PinStatusChanged SDCardRemoved;
        public event Globals.PinStatusChanged SDCardAdded;
        
        public SDStorage()
        {
            this.sdCardStatusPort = new InterruptPort((Cpu.Pin)57, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
            this.sdCardStatusPort.OnInterrupt += new NativeEventHandler(sdCardStatusPort_OnInterrupt);
            this.Root = new DirectoryInfo(@"\SD\");
        }
        public FileStream getStream(string fileName)
        {
            bool exists = false;
            if (!this.Root.Exists)
            {
                return null;
            }
            foreach (FileInfo file in Root.GetFiles())
            {
                if (file.Name == fileName)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                FileStream fs = File.Create(this.Root.FullName + @"\" + fileName);
                return fs;
            }
            return null;
        }
        private void sdCardStatusPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            bool there = sdCardStatusPort.Read();
            if (there)
            {
                OnSDCardAdded(time);
            }
            else
            {
                OnSDCardRemoved(time);
            }
        }
        private void OnSDCardRemoved(DateTime t)
        {
            if(this.SDCardRemoved!=null)
            {
                this.SDCardRemoved(t);
            }
        }
        private void OnSDCardAdded(DateTime t)
        {
            if (this.SDCardAdded != null)
            {
                this.SDCardAdded(t);
            }
        }
    }
}
