using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GintySoft.KinectLib;
using Microsoft.Kinect;

namespace GintySoft.Kinect
{
    public class UIHelpers
    {
        
        public static BitmapSource VideoToBitmapSource(VideoImage Image)
        {
            BitmapSource bmap = BitmapSource.Create(
            Image.ImageFrame.Width,
            Image.ImageFrame.Height,
            96, 96,
            PixelFormats.Bgr32,
            null,
            Image.Data,
            Image.ImageFrame.Width * Image.ImageFrame.BytesPerPixel);

            return bmap;
        }

        internal static BitmapSource PlayerDepthToBitmapSource(Depth playerDepthFrame, bool p)
        {
            short[] vals = new short[playerDepthFrame.PlayerDepthFrame.PlayerDepths.Length];

            int ctnr = 0;
            foreach (PlayerDepth pd in playerDepthFrame.PlayerDepthFrame.PlayerDepths)
            {
                if (p)
                {
                    vals[ctnr] = (short)pd.Player;
                }
                else
                {
                    vals[ctnr] = (short)pd.Depth;
                }
            }
            BitmapSource bmap = BitmapSource.Create(
            playerDepthFrame.DepthFrame.Width,
            playerDepthFrame.DepthFrame.Height,
            96, 96,
            PixelFormats.Gray16,
            null,
            vals,
            playerDepthFrame.DepthFrame.Width*2);

            return bmap;
        }
    }
}
