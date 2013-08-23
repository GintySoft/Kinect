using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace GintySoft.Kinectduino
{
    public class Button
    {
        public InterruptPort OnboardButton { get; set; }

        public event Globals.PinStatusChanged ButtonPressed;
        public event Globals.PinStatusChanged ButtonReleased;

        public bool IsPressed
        {
            get
            {
                return !this.OnboardButton.Read();
            }
        }

        public Button()
        {
            this.OnboardButton = new InterruptPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            this.OnboardButton.OnInterrupt += new NativeEventHandler(OnboardButton_OnInterrupt);
        }

        private void OnboardButton_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (data2 == 0)
            {//pressed
                OnPressed(time);
            }
            else if (data2 == 1)
            {//not presssed
                OnReleased(time);
            }
        }
        private void OnPressed(DateTime t)
        {
            if (this.ButtonPressed != null)
            {
                this.ButtonPressed(t);
            }
        }
        private void OnReleased(DateTime t)
        {
            if (this.ButtonReleased != null)
            {
                this.ButtonReleased(t);
            }
        }
    }
}
