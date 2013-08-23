using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public static class KinectManager
    {
        static KinectManager()
        {
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
        }

        private static void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            //nothing yet
        }

        public static KinectSensor GetFirstOrDefaultConnectedKinectSensor()
        {
            List<KinectSensor> sensors = GetAllConnectedKinectSensors();
            if (sensors.Count > 0)
            {
                return sensors[0];
            }
            return null;
        }
        public static List<KinectSensor> GetAllKinectSensors()
        {
            return KinectSensor.KinectSensors.ToList();
        }
        public static List<KinectSensor> GetAllConnectedKinectSensors()
        {
            var sensors = from sensorToCheck in KinectSensor.KinectSensors
                           where sensorToCheck.Status == KinectStatus.Connected
                           select sensorToCheck;

            return sensors.ToList();
        }
    }
}
