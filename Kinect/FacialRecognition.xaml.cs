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
using Emgu.CV.Structure;
using Emgu.CV.WPF;
using Emgu.CV;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;

namespace GintySoft.Kinect
{
    /// <summary>
    /// Interaction logic for FacialRecognition.xaml
    /// </summary>
    public partial class FacialRecognition : UserControl
    {
        private KinectLib.BitmapGenerator B;
        private HaarCascade haarCascade;
        private int m_counter = 0;
        private delegate void Faker();
        public FacialRecognition(KinectLib.BitmapGenerator b)
        {
            InitializeComponent();
            haarCascade = new HaarCascade(@"haarcascade_frontalface_default.xml");
            B = b;

            B.NewBitmapReady += new KinectLib.BitmapGenerator.BitmapReadyDelegate(B_NewBitmapReady);
        }

        private void B_NewBitmapReady(System.Drawing.Bitmap b)
        {
            if (m_counter > 200)
            {
                Image<Bgr, byte> currentFrame = new Image<Bgr, byte>(b);
         
                if (currentFrame != null)
                {
                    Image<Gray, byte> grayFrame = currentFrame.Convert<Gray, byte>();
                    var detectFaced = haarCascade.Detect(grayFrame);
                    foreach (var face in detectFaced)
                    {
                        currentFrame.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
                    }
                    this.Dispatcher.BeginInvoke(new Faker(delegate
                    {
                        BitmapSource bms = BitmapSourceConvert.ToBitmapSource(currentFrame);
                        image1.Source = bms;
                    }), DispatcherPriority.Background, new object[] { });
                }

                m_counter = 0;
            }
            else
            {
                m_counter++;
            }
        }
    }
}
