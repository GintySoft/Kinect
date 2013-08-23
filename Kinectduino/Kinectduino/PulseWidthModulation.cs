using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace GintySoft.Kinectduino
{
    public class PulseWidthModulation
    {
        public enum Type
        {
            DutyCyle,
            Values
        }
        private PWM P { get; set; }
        public uint Period { get; set; }
        public uint Duration { get; set; }
        public uint DutyCyle { get; set; }
        public Type PulseType { get; set; }

        public PulseWidthModulation(uint period, uint duration)
        {
            this.P = new PWM(Pins.GPIO_PIN_D5);
            this.Period = period;
            this.Duration = duration;
            this.PulseType = Type.Values;
            this.DutyCyle = this.Duration / this.Period;
        }
        public PulseWidthModulation(uint dutyCycle)
        {
            this.DutyCyle = dutyCycle;
            this.PulseType = Type.DutyCyle;
        }
        public void start()
        {
            if (this.PulseType == Type.Values)
            {
                this.P.SetPulse(this.Period, this.Duration);
            }
            else if (this.PulseType == Type.DutyCyle)
            {
                this.P.SetDutyCycle(this.DutyCyle);
            }
        }
    }
}
