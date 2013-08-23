using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Speech;
using Microsoft.Speech.Recognition;
using Microsoft.Kinect;
using System.Speech.AudioFormat;

namespace GintySoft.KinectLib
{
    public class Kinect : IDisposable
    {        
        private const int BUFFER = 2;

        private CameraManager m_cameraManager;
        private PerSecondCalculator VideoFPS { get; set; }
        private PerSecondCalculator DepthFPS { get; set; }
        private PerSecondCalculator SkeletonFPS { get; set; }
        public volatile bool ShouldRun = true;
        public event StatusChangedDelegate StatusChanged;
        public delegate void StatusChangedDelegate(StatusChangedEventArgs eventArgs);
        public event AudioStreamRecorded NewRecordedAudio;
        public delegate void AudioStreamRecorded(byte[] audio);
        public event NewSpeechRecognizedResultDelegate NewSpeechRecognizedResult;
        public delegate void NewSpeechRecognizedResultDelegate(RecognitionResult result);
        public SoundSourceAngleChangedDelegate SoundSourceAngleChanged;
        public delegate void SoundSourceAngleChangedDelegate(SoundSource s);
        public event AudioBeamAngleChangedDelegate AudioBeamAngleChanged;
        public delegate void AudioBeamAngleChangedDelegate(double angle);
        public event VideoFrameReadyDelegate NewVideoFrame;
        public delegate void VideoFrameReadyDelegate(VideoImage planerImage);
        public event SkeletonFrameReadyDelegate NewSkeletonFrame;
        public delegate void SkeletonFrameReadyDelegate(Skel skeletonImage);
        public event DepthFrameReadyDelegate NewDepthFrame;
        public delegate void DepthFrameReadyDelegate(Depth depthImage);
        public event AllFramesReadyDelegate AllFramesReady;
        public delegate void AllFramesReadyDelegate(AllFrames frames);
        public KinectSensor KinectDevice { get; set; }
        private CameraManager CameraManager
        {
            get
            {
                return m_cameraManager;
            }
            set
            {
                m_cameraManager = value;
            }
        }
        public Kinect(KinectSensor kinect, bool speechRecoginition = false, bool recordAudio = false)
        {
            this.KinectDevice = kinect;

            if (!(this.KinectDevice.Status == KinectStatus.Connected))
            {
                throw new Exception("Kinect Not Connected");
            }

            TransformSmoothParameters tParams = new TransformSmoothParameters
            {
                 
            };
            this.KinectDevice.SkeletonStream.Enable(tParams);
            this.KinectDevice.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            this.KinectDevice.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            
            this.start();

            this.CameraManager = new CameraManager(this.KinectDevice);
            this.VideoFPS = new PerSecondCalculator();
            this.DepthFPS = new PerSecondCalculator();
            this.SkeletonFPS = new PerSecondCalculator();            

            if (speechRecoginition)
            {
                var speechthread = new Task(new Action(speechRecognitionGenerator));
                speechthread.Start();
            }
            else if(recordAudio)
            {
                var audiothread = new Thread(new ThreadStart(recordAudioStream));
                audiothread.SetApartmentState(ApartmentState.MTA);
                audiothread.Start();
            }
        }
        private void addHandlers()
        {
            this.removeHandlers();//for safety, dont want to be doubling up handlers...
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            this.KinectDevice.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(Kinect_AllFramesReady);
            this.KinectDevice.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(Kinect_ColorFrameReady);
            this.KinectDevice.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(Kinect_DepthFrameReady);
            this.KinectDevice.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(Kinect_SkeletonFrameReady);
            this.KinectDevice.AudioSource.SoundSourceAngleChanged += new EventHandler<SoundSourceAngleChangedEventArgs>(AudioSource_SoundSourceAngleChanged);
            this.KinectDevice.AudioSource.BeamAngleChanged += new EventHandler<BeamAngleChangedEventArgs>(AudioSource_BeamAngleChanged);
        }
        private void removeHandlers()
        {
            KinectSensor.KinectSensors.StatusChanged -=KinectSensors_StatusChanged;
            this.KinectDevice.AllFramesReady -=Kinect_AllFramesReady;
            this.KinectDevice.ColorFrameReady -=Kinect_ColorFrameReady;
            this.KinectDevice.DepthFrameReady-=Kinect_DepthFrameReady;
            this.KinectDevice.SkeletonFrameReady -=Kinect_SkeletonFrameReady;
            this.KinectDevice.AudioSource.SoundSourceAngleChanged -= AudioSource_SoundSourceAngleChanged;
            this.KinectDevice.AudioSource.BeamAngleChanged -= AudioSource_BeamAngleChanged;
        }
        public void stop()
        {
            this.removeHandlers();
            if (this.KinectDevice.IsRunning)
            {
                this.KinectDevice.Stop();
                this.KinectDevice.AudioSource.Stop();
            }
        }
        public void start()
        {
            this.addHandlers();
            if (!this.KinectDevice.IsRunning)
            {
                this.KinectDevice.Start();
                this.KinectDevice.AudioSource.Start();
            }
        }
        private void  KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (this.StatusChanged != null)
            {
                if (e.Sensor.UniqueKinectId == this.KinectDevice.UniqueKinectId)
                {
                    this.StatusChanged(e);
                }
            }
        }
        private void AudioSource_BeamAngleChanged(object sender, BeamAngleChangedEventArgs e)
        {
            if (this.AudioBeamAngleChanged != null)
            {
                this.AudioBeamAngleChanged(e.Angle);
            }
        }
        private void AudioSource_SoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
        {
            if (this.SoundSourceAngleChanged != null)
            {
                var soundS = new SoundSource
                {
                     Angle = e.Angle,
                     ConfidenceLevel = e.ConfidenceLevel
                };
                this.SoundSourceAngleChanged(soundS);
            }
        }
        private void Kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            if (this.NewSkeletonFrame != null)
            {
                using (SkeletonFrame skelFrame = e.OpenSkeletonFrame())
                {
                    if (skelFrame != null)
                    {
                        Skel skel = new Skel(skelFrame);
                        this.NewSkeletonFrame(skel);
                        this.SkeletonFPS.hit();
                    }
                }
            }
        }
        private void Kinect_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            if (this.NewDepthFrame != null)
            {
                using (DepthImageFrame depthIF = e.OpenDepthImageFrame())
                {
                    if (depthIF != null)
                    {
                        Depth d = new Depth(depthIF);
                        this.NewDepthFrame(d);
                        this.DepthFPS.hit();
                    }
                }
            }
        }
        private void Kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (this.NewVideoFrame != null)
            {
                using (ColorImageFrame color = e.OpenColorImageFrame())
                {
                    if (color != null)
                    {
                        VideoImage vid = new VideoImage(color);
                        this.NewVideoFrame(vid);
                        this.VideoFPS.hit();
                    }
                }
            }
        }
        private void Kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            if (AllFramesReady != null)
            {
                using(var skel = e.OpenSkeletonFrame())
                using(var color = e.OpenColorImageFrame())
                using (var depth = e.OpenDepthImageFrame())
                {
                    AllFrames all = new AllFrames
                    {
                        Skeleton = new Skel(skel),
                        Image = new VideoImage(color),
                        Depth = new Depth(depth)
                    };
                    this.AllFramesReady(all);
                }
            }   
        }
        private void recordAudioStream()
        {
            int bufferSize = 1024;
            using (var audioStream = this.KinectDevice.AudioSource.Start())
            {
                while (ShouldRun)
                {
                    int cnt = 0;
                    var buffer = new byte[bufferSize];
                    while(cnt < bufferSize && ShouldRun)
                    {
                        cnt += audioStream.Read(buffer, 0, bufferSize);
                    }
                    if (NewRecordedAudio != null)
                    {
                        NewRecordedAudio(buffer);
                    }
                }
            }
        }
        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name,
                  StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        private void speechRecognitionGenerator()
        {            
            RecognizerInfo ri = GetKinectRecognizer();
            using (var sre = new SpeechRecognitionEngine(ri.Id))
            {
                var options = new Choices();
                options.Add("password");
                options.Add("oscar");
                options.Add("zeus");
                var gb = new GrammarBuilder();
                //Specify the culture to match the recognizer in case we are running in a different culture.                                 
                gb.Culture = ri.Culture;
                gb.Append(options);
                var g = new Grammar(gb);
                sre.LoadGrammar(g);
                sre.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(sre_SpeechHypothesized);
                sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sre_SpeechRecognitionRejected);
                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
                Stream audioStream = this.KinectDevice.AudioSource.Start();
                Microsoft.Speech.AudioFormat.SpeechAudioFormatInfo info  = new Microsoft.Speech.AudioFormat.SpeechAudioFormatInfo(Microsoft.Speech.AudioFormat.EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null);
                sre.SetInputToAudioStream(audioStream, info);
                sre.RecognizeAsync(RecognizeMode.Multiple);
                while (ShouldRun)
                {  
                    Thread.Sleep(1000);
                }
                sre.RecognizeAsyncStop();
            }            
        }
        private void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            raiseSpeechRecognized(e.Result);
        }
        private void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            raiseSpeechRecognized(e.Result);
        }
        private void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            raiseSpeechRecognized(e.Result);
        }
        private void raiseSpeechRecognized(RecognitionResult res)
        {
            if (this.NewSpeechRecognizedResult != null)
            {
                this.NewSpeechRecognizedResult(res);
            }
        }          
        public bool AimCameraDown()
        {
            return this.CameraManager.decreaseAngleDown();
        }
        public bool AimCameraUp()
        {
            return this.CameraManager.increaseAngleUp();         
        }
        public int VideoFramesPerSecond()
        {
            return this.VideoFPS.PerSecond;
        }
        public int SkeletonPerSecond()
        {
            return this.SkeletonFPS.PerSecond;
        }
        public int DepthFramesPerSecond()
        {
            return this.DepthFPS.PerSecond;
        }
        public void Dispose()
        {
            ShouldRun = false;
            if (this.KinectDevice != null)
            {
                this.removeHandlers();
                this.KinectDevice.Stop();
                this.KinectDevice.AudioSource.Stop();
            }
        }
    }
}
