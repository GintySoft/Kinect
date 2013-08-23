using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public class VideoImage
    {
        private ColorImageFrame m_imageFrame;
        private byte[] m_data = null;

        public ColorImageFrame ImageFrame
        {
            get
            {
                return this.m_imageFrame;
            }
        }
        public byte[] Data
        {
            get
            {
                if (m_data == null)
                {
                    m_data = new byte[this.ImageFrame.PixelDataLength];
                    this.ImageFrame.CopyPixelDataTo(m_data);
                }
                return m_data;
            }
        }
        public VideoImage(ColorImageFrame frame)
        {
            this.m_imageFrame = frame;
        }
    }
}
