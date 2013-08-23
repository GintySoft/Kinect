using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GintySoft.KinectLib
{
    public class PerSecondCalculator
    {
        private long m_total = 0;
        private long m_last = 0;
        private DateTime m_lastTime = DateTime.Now;
        private object m_totalLock = new object();
        public int PerSecond { get; private set; }

        public PerSecondCalculator()
        {
            PerSecond = 0;
        }
        public void hit()
        {
            lock (m_totalLock)
            {
                m_total++;
                var cur = DateTime.Now;
                if (cur.Subtract(m_lastTime) > TimeSpan.FromSeconds(1))
                {
                    long diff = m_total - m_last;
                    m_last = m_total;
                    m_lastTime = cur;
                    this.PerSecond = (int)diff;
                }
            }
        }
    }
}
