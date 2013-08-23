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
    public class ImageFrameViewModel : KinectBase, INotifyPropertyChanged
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

        private BitmapSource m_image;

        public BitmapSource Image
        {
            get
            {
                return m_image;
            }
            set
            {
                m_image = value;

                OnPropertyChanged("Image");
            }
        }

        public ImageFrameViewModel(KinectLib.Kinect kinect) : base(kinect)
        {

        }

        protected override void Kinect_NewVideoFrame(VideoImage planerImage)
        {
            this.Image = UIHelpers.VideoToBitmapSource(planerImage);   
        }
    }
}
