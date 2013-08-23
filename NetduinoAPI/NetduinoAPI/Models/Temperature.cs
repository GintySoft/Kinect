using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetduinoAPI.Models
{
    public class Temperature
    {
        public static List<Temperature> temps = new List<Temperature>();

        public double TempVal { get; set; }
        public string DeviceID { get; set; }
        public double Time { get; set; }
        public string ID { get; set; }

        public Temperature(double temp, double timestamp, string deviceID)
        {
            this.DeviceID = deviceID;            
            this.TempVal = temp;
            this.Time = timestamp;
            this.ID = Guid.NewGuid().ToString();
        }
    }
}