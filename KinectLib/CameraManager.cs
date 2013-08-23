using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public class CameraManager
    {
        public const int MOTION_COUNT_MAX = 2;

        private Timer m_resetTimer = new Timer();
        private object m_lock = new object();
        private int m_motionCount = 0;
        public KinectSensor Kinect { get; set; }

        public CameraManager(KinectSensor k)
        {
            resetTimer();
            m_resetTimer.Elapsed += new ElapsedEventHandler(m_resetTimer_Elapsed);
        }
        private void resetTimer()
        {
            m_resetTimer.AutoReset = false;
            m_resetTimer.Interval = 1*1000;//1 second
        }
        private void m_resetTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_resetTimer.Enabled = false;
            m_resetTimer.Stop();
            m_motionCount = 0;
        }

        public bool increaseAngleUp()
        {
            return changeAngle(5);
        }
        public bool decreaseAngleDown()
        {
            return changeAngle(-5);
        }

        private bool changeAngle(int val)
        {
            lock (m_lock)
            {
                if (m_resetTimer.Enabled)
                {
                    if (m_motionCount < MOTION_COUNT_MAX)
                    {
                       return changeAngleNoLock(val);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    bool ret = false;
                    if (m_motionCount < MOTION_COUNT_MAX)
                    {
                        ret = changeAngleNoLock(val);
                    }

                    resetTimer();
                    m_resetTimer.Enabled = true;
                    m_resetTimer.Start();

                    return ret;
                }
            }
        }
        private bool changeAngleNoLock(int val)
        {
            int original = this.Kinect.ElevationAngle;
            int newVal = original + val;
            if (newVal < this.Kinect.MaxElevationAngle && newVal > this.Kinect.MinElevationAngle)
            {
                try
                {
                    this.Kinect.ElevationAngle = newVal;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            int newAngle = this.Kinect.ElevationAngle;

            if (newAngle != original)
            {
                m_motionCount++;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
