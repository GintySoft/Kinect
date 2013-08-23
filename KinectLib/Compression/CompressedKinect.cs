using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
namespace GintySoft.KinectLib
{
    public class CompressedKinect : KinectBase
    {
        public event NewCompressedKinectImageFrameDelegate NewCompressedKinectDepthFrame;
        public event NewCompressedKinectImageFrameDelegate NewCompressedKinectVideoFrame;
        public event NewCompressedSkeletonFrameDelegate NewCompressedSkeletonFrame;
        public event NewCompressedAudioStreamDelegate NewCompressedAudioStream;
        public event NewCompressedAudioAngleDelegate NewCompressedAudioAngle;

        public delegate void NewCompressedAudioStreamDelegate(byte[] compressedBytes, byte [] originalBytes);
        public delegate void NewCompressedAudioAngleDelegate(byte[] compressedAngle, double audioDirectionAngle);
        public delegate void NewCompressedKinectImageFrameDelegate(byte[] bytes, ImageFrame frame);
        public delegate void NewCompressedSkeletonFrameDelegate(byte[] bytes, SkeletonFrame frame);

        public CompressedKinect(Kinect k)
            : base(k)
        {
        }

   
        protected override void Kinect_NewRecordedAudio(byte[] audio)
        {
            base.Kinect_NewRecordedAudio(audio);
            if (NewCompressedAudioStream != null)
            {
                Compression<byte[]> comp = new Compression<byte[]>();
                byte[] compressedValue = comp.GzipCompress(audio);
                NewCompressedAudioStream(compressedValue, audio);
            }
        }
        protected override void Kinect_NewSkeletonFrame(Skel skeletonImage)
        {
            base.Kinect_NewSkeletonFrame(skeletonImage);

            if (NewCompressedSkeletonFrame != null)
            {
                Compression<SkeletonFrame> comp = new Compression<SkeletonFrame>();
                byte[] compressedVal = comp.GzipCompress(skeletonImage.Skeleton);
                NewCompressedSkeletonFrame(compressedVal, skeletonImage.Skeleton);
            }
        }
        protected override void Kinect_NewVideoFrame(VideoImage videoImage)
        {
            base.Kinect_NewVideoFrame(videoImage);

            if (NewCompressedKinectVideoFrame != null)
            {
                Compression<ImageFrame> comp = new Compression<ImageFrame>();
                byte[] compressedVal = comp.GzipCompress(videoImage.ImageFrame);
                NewCompressedKinectVideoFrame(compressedVal, videoImage.ImageFrame);
            }
        }
        protected override void Kinect_NewDepthFrame(Depth depthImage)
        {
            base.Kinect_NewDepthFrame(depthImage);

            if (NewCompressedKinectDepthFrame != null)
            {
                Compression<DepthImageFrame> comp = new Compression<DepthImageFrame>();
                byte[] compressedVal = comp.GzipCompress(depthImage.DepthFrame);
                NewCompressedKinectDepthFrame(compressedVal, depthImage.DepthFrame);
            }
        }
    }
}
