using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Net;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace GintySoft.Kinectduino
{
    public class Program
    {
        static LED le = new LED();
        public static void Main()
        {            
            Temperature t = new Temperature(new TimeSpan(0, 0, 1));
            t.NewTemperatureFahrenheit += new Temperature.NewTemperatureDelegate(t_NewTemperatureFahrenheit);
 
            Thread.Sleep(Timeout.Infinite);
        }

        static void t_NewTemperatureFahrenheit(double tempVal)
        {
            Debug.Print(tempVal.ToString());
            PachubeHttp http = new PachubeHttp();
            http.Post(tempVal.ToString());
        }

        static void g_NewGPSLocation(string latitude, string longitute)
        {
            if (le.State == false)
            {
                le.State = true;
            }
            else
            {
                le.State = false;
            }
        }

    }
}
