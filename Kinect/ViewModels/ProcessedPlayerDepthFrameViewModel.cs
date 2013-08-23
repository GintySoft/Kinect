using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using Microsoft.Kinect;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GintySoft.KinectLib;

namespace GintySoft.Kinect
{
    public class ProcessedPlayerDepthFrameViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        private BitmapSource m_playerimage;
        private BitmapSource m_depthimage;
        const float MaxDepthDistance = 4095; 
        const float MinDepthDistance = 800; 
        const float MaxDepthDistanceOffset = MaxDepthDistance - MinDepthDistance; 
        public BitmapSource DepthImage
        {
            get
            {
                return m_depthimage;
            }
            set
            {
                m_depthimage = value;

                OnPropertyChanged("DepthImage");
            }
        }
        public BitmapSource PlayerImage
        {
            get
            {
                return m_playerimage;
            }
            set
            {
                m_playerimage = value;

                OnPropertyChanged("PlayerImage");
            }
        }

        public ProcessedPlayerDepthFrameViewModel(KinectLib.Kinect k)
        {
            k.NewDepthFrame += new KinectLib.Kinect.DepthFrameReadyDelegate(k_NewDepthFrame);
        }
        public static Byte CalculateIntensityFromDepth(Int32 distance)
        {            
            return (Byte)(255 - (255 * Math.Max(distance - MinDepthDistance, 0) / (MaxDepthDistanceOffset)));
        }
        private Byte[] GenerateColoredBytes(PlayerDepth[] depths, int height, int width)
        { 
            Byte[] pixels = new Byte[height * width * 4]; 
            const Int32 BlueIndex = 0; const Int32 GreenIndex = 1; 
            const Int32 RedIndex = 2; 
            for (Int32 depthIndex = 0, colorIndex = 0; depthIndex < depths.Length && colorIndex < pixels.Length; depthIndex++, colorIndex += 4)
            {
                PlayerDepth pd = depths[depthIndex];
                Byte intensity = CalculateIntensityFromDepth(pd.Depth); 
                pixels[colorIndex + BlueIndex] = intensity; 
                pixels[colorIndex + GreenIndex] = intensity; 
                pixels[colorIndex + RedIndex] = intensity; 
            } 
            return pixels;
        }
        void k_NewDepthFrame(Depth depthImage)
        {
            byte[] pixels = GenerateColoredBytes(depthImage.PlayerDepthFrame.PlayerDepths, depthImage.DepthFrame.Height, depthImage.DepthFrame.Width);
                this.DepthImage =  BitmapSource.Create(depthImage.DepthFrame.Width, depthImage.DepthFrame.Height, 96, 96, PixelFormats.Bgr32, null, pixels, depthImage.DepthFrame.Width * 4);  
                // this.PlayerImage = UIHelpers.PlayerDepthToBitmapSource(depthImage, false);
         
        }
    }
}
