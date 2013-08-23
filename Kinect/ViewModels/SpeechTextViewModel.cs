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
    public class SpeechTextViewModel : INotifyPropertyChanged
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

        private string m_text;
        private KinectLib.Kinect m_kinect;

        public string Text
        {
            get
            {
                return m_text;
            }
            private set
            {
                this.m_text = value;

                OnPropertyChanged("Text");
            }
        }

        public SpeechTextViewModel(KinectLib.Kinect kinect)
        {
            m_kinect = kinect;

            m_kinect.NewSpeechRecognizedResult += new KinectLib.Kinect.NewSpeechRecognizedResultDelegate(m_kinect_NewSpeechRecognizedResult);
        }

        void m_kinect_NewSpeechRecognizedResult(Microsoft.Speech.Recognition.RecognitionResult result)
        {
            this.Text = result.Text;
        }
    }
}
