using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public class ProcessedPlayerDepthFrame
    {
        private PlayerDepth[] m_playerDepthArray = null;

        public PlayerDepth[] PlayerDepths
        {
            get
            {
                return m_playerDepthArray;
            }
            private set
            {
                this.m_playerDepthArray = value;
            }
        }

        public ProcessedPlayerDepthFrame(short[] shorts)
        {
            this.PlayerDepths = new PlayerDepth[shorts.Length];

            for (int i = 0; i < shorts.Length; i++)
            {
                short val = shorts[i];
                PlayerDepth pd = new PlayerDepth(val);
                this.PlayerDepths[i] = pd;
            }
        }
    }
}
