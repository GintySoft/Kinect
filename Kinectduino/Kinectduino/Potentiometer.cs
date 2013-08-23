using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace GintySoft.Kinectduino
{
    public class Potentiometer
    {
        public event NewVoltageDelegate NewVoltage;
        public delegate void NewVoltageDelegate(double voltage);

        private const double MAX_VOLTAGE = 3.3;
        private const int MAX_ADC_VALUE = 1023;

        private Timer ReadTimer;
        private AnalogInput VoltagePort { get; set; }
        private OutputPort HighPort { get; set; }
        private OutputPort LowPort { get; set; }
        private TimeSpan SamplePeriod { get; set; }
        public Potentiometer(TimeSpan samplePeriod)
        {
            this.SamplePeriod = samplePeriod;
            this.HighPort = new OutputPort(Pins.GPIO_PIN_A0, true);
            this.LowPort = new OutputPort(Pins.GPIO_PIN_A2, false);
            this.VoltagePort = new AnalogInput(Pins.GPIO_PIN_A1);
            this.ReadTimer = new Timer(new TimerCallback(readVoltage), null, new TimeSpan(0, 0, 1), samplePeriod);
        }

        private void readVoltage(object sender)
        {
            var rawValue = this.VoltagePort.Read();
            double value = (rawValue * MAX_VOLTAGE) / MAX_ADC_VALUE;
            raiseNewVoltage(value);
        }
        private void raiseNewVoltage(double voltage)
        {
            if (NewVoltage != null)
            {
                NewVoltage(voltage);
            }
        }
    }
}
