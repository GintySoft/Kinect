using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public class Depth
    {
        private DepthImageFrame m_depthFrame;
        private short[] m_pixelData = null;
        private ProcessedPlayerDepthFrame m_playerDepthFrame = null;

        public DepthImageFrame DepthFrame
        {
            get
            {
                return m_depthFrame;
            }
        }

        public short[] DepthData
        {
            get
            {
                if (m_pixelData == null)
                {
                    m_pixelData = new short[this.DepthFrame.PixelDataLength];
                    this.DepthFrame.CopyPixelDataTo(this.m_pixelData);
                }
                return m_pixelData;
            }
        }
        public ProcessedPlayerDepthFrame PlayerDepthFrame
        {
            get
            {
                if (this.m_playerDepthFrame == null)
                {
                    this.m_playerDepthFrame = new ProcessedPlayerDepthFrame(this.DepthData);
                }
                return this.m_playerDepthFrame;             
            }
        }

        public Depth(DepthImageFrame frame)
        {
            this.m_depthFrame = frame;
        }
    }
}
