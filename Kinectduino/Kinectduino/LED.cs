using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace GintySoft.Kinectduino
{
    public class LED
    {
        private OutputPort OnboardLED { get; set; }
        public bool State
        {
            get
            {
                return this.OnboardLED.Read();
            }
            set
            {
                this.OnboardLED.Write(value);
            }
        }
        public LED()
        {
            this.OnboardLED = new OutputPort(Pins.ONBOARD_LED, false);
        }

        public void blinkOnTimer(TimeSpan timeBetweenFlashes)
        {
        }
        public void blink(TimeSpan timeBetweenFlashes)
        {
            this.OnboardLED.Write(true);
            Thread.Sleep(timeBetweenFlashes.Milliseconds);
            this.OnboardLED.Write(false);
        }
    }
}
