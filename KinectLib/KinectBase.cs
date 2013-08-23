using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public class KinectBase
    {
        private Kinect m_kinect;

        public Kinect Kinect
        {
            get
            {
                return m_kinect;
            }
            private set
            {
                this.m_kinect = value;
            }
        }

        public KinectBase(Kinect k)
        {
            this.Kinect = k;
            this.Kinect.NewSpeechRecognizedResult += new KinectLib.Kinect.NewSpeechRecognizedResultDelegate(Kinect_NewSpeechRecognizedResult);
            this.Kinect.NewDepthFrame += new KinectLib.Kinect.DepthFrameReadyDelegate(Kinect_NewDepthFrame);
            this.Kinect.NewSkeletonFrame += new KinectLib.Kinect.SkeletonFrameReadyDelegate(Kinect_NewSkeletonFrame);
            this.Kinect.NewVideoFrame += new KinectLib.Kinect.VideoFrameReadyDelegate(Kinect_NewVideoFrame);
            this.Kinect.NewRecordedAudio += new KinectLib.Kinect.AudioStreamRecorded(Kinect_NewRecordedAudio);
        }

        protected virtual void Kinect_NewSpeechRecognizedResult(Microsoft.Speech.Recognition.RecognitionResult result)
        {
            
        }
        protected virtual void Kinect_NewRecordedAudio(byte[] audio)
        {

        }
        protected virtual void Kinect_NewVideoFrame(VideoImage videoImage)
        {
            
        }
        protected virtual void Kinect_NewSkeletonFrame(Skel skeletonImage)
        {
          
        }
        protected virtual void Kinect_NewDepthFrame(Depth depthImage)
        {
          
        }
    }
}
