using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace GintySoft.Kinectduino
{
    public class Temperature
    {
        public event NewTemperatureDelegate NewTemperatureVoltage;
        public event NewTemperatureDelegate NewTemperatureCelsius;
        public event NewTemperatureDelegate NewTemperatureFahrenheit;

        public delegate void NewTemperatureDelegate(double tempVal);

        private const double MAX_VOLTAGE = 3.3;
        private const int MAX_ADC_VALUE = 1023;

        private Timer ReadTimer;
        private AnalogInput TemperaturePort { get; set; }
        private TimeSpan SamplePeriod { get; set; }

        public Temperature(TimeSpan samplePeriod)
        {
            this.SamplePeriod = samplePeriod;
            this.TemperaturePort = new AnalogInput(Pins.GPIO_PIN_A1);
            this.ReadTimer = new Timer(new TimerCallback(readTemperature), null, new TimeSpan(0, 0, 1), samplePeriod);
        }
        private void readTemperature(object sender)
        {
            var rawValue = this.TemperaturePort.Read();
            double value = (rawValue * MAX_VOLTAGE) / MAX_ADC_VALUE;
            raiseNewTemperature(value);
        }
        private void raiseNewTemperature(double voltage)
        {
            if (NewTemperatureVoltage != null)
            {
                NewTemperatureVoltage(voltage);
            }
            if (NewTemperatureFahrenheit != null)
            {
                var fah = voltageToFah(voltage);
                NewTemperatureFahrenheit(fah);
            }
            if (NewTemperatureCelsius != null)
            {
                var cel = voltageToCel(voltage);
                NewTemperatureCelsius(cel);
            }
        }
        private double voltageToCel(double voltage)
        {
            var cel = voltage * 100;
            cel = cel - 273.15;
            return cel;
        }
        private double voltageToFah(double voltage)
        {            
            var fah = voltage * 100;
            fah = fah - 273.15;
            fah = (9 / 5) * fah + 32;
            return fah;
        }
    }
}
