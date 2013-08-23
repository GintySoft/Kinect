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
using Microsoft.Kinect;
using GintySoft.KinectLib;

namespace GintySoft.Kinect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectLib.Kinect m_k = null;

        public MainWindow()
        {
            InitializeComponent();
            KinectSensor k = KinectManager.GetFirstOrDefaultConnectedKinectSensor();

            if(k==null)
            {
                MessageBox.Show("No Kinects");
                Environment.Exit(0);
            }
            m_k = new KinectLib.Kinect(k, true);
            
            //AzureStorage az = new AzureStorage(m_k);
            //KinectAudioRecorder rec = new KinectAudioRecorder(k, @"C:\Users\cmcginty\Desktop\kinect\" + Guid.NewGuid().ToString() + ".wav", 10);
            //rec.NewAudioRecordingDone += new KinectAudioRecorder.AudioRecordingDone(rec_NewAudioRecordingDone);
            //SkeletalViewModel viewModel = new SkeletalViewModel(m_k);
            //ImageFrameViewModel imageviewmodel = new ImageFrameViewModel(m_k);
            ProcessedPlayerDepthFrameViewModel processdepth = new ProcessedPlayerDepthFrameViewModel(m_k);
            //SpeechTextViewModel speech = new SpeechTextViewModel(m_k);

            //this.speech.DataContext = speech;
            this.PlayerDepth.DataContext = processdepth;
            //this.Video.DataContext = imageviewmodel;
            //this.Skeletal.DataContext = viewModel;

            //KinectLib.BitmapGenerator b = new KinectLib.BitmapGenerator(k);

            //FacialRecognition f = new FacialRecognition(b);

            //this.facialCanvas.Children.Add(f);

            ////KinectLib.AzureStorage azure = new KinectLib.AzureStorage(k);
            //StorageOptions so = new StorageOptions
            //{
            //    ColorMax = 40,
            //    SkeltonMax = 40,
            //    DepthMax = 40,
            //    StoreColor = true,
            //    StoreSkeleton = true,
            //    StoreDepth = true
            //};
            //MemoryStorage ms = new MemoryStorage(k, so);


            //
           // bool val1 =k.AimCameraDown();
           // System.Threading.Thread.Sleep(1000);
           // bool val2 =k.AimCameraDown();
            //System.Threading.Thread.Sleep(1000);
           // bool val3 = k.AimCameraDown();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            m_k.Dispose();
        }
    }
}
