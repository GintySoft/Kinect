using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public class MemoryStorage : KinectStorage
    {
        private ConcurrentQueue<ImageFrame> ColorImages { get; set; }
        private ConcurrentQueue<DepthImageFrame> DepthImages { get; set; }
        private ConcurrentQueue<SkeletonFrame> SkeletonFrames { get; set; }

        public MemoryStorage(Kinect k, StorageOptions options)
            : base(k, options)
        {
            if (this.Options.StoreSkeleton)
            {
                this.SkeletonFrames = new ConcurrentQueue<SkeletonFrame>();
            }
            if (this.Options.StoreDepth)
            {
                this.DepthImages = new ConcurrentQueue<DepthImageFrame>();
            }
            if (this.Options.StoreColor)
            {
                this.ColorImages = new ConcurrentQueue<ImageFrame>();
            }
        }
        protected override void Kinect_NewVideoFrame(VideoImage planerImage)
        {
            base.Kinect_NewVideoFrame(planerImage);
            if (Options.StoreColor)
            {
                this.ColorImages.Enqueue(planerImage.ImageFrame);
                if (this.ColorImages.Count > this.Options.ColorMax)
                {
                    ImageFrame dead;
                    this.ColorImages.TryDequeue(out dead);
                }
            }
        }
        protected override void  Kinect_NewSkeletonFrame(Skel skeletonImage)
        {
 	         base.Kinect_NewSkeletonFrame(skeletonImage);

            if(this.Options.StoreSkeleton)
            {
                this.SkeletonFrames.Enqueue(skeletonImage.Skeleton);
                if(this.SkeletonFrames.Count > this.Options.SkeltonMax)
                {
                    SkeletonFrame dead;
                    this.SkeletonFrames.TryDequeue(out dead);
                }
            }
        }
        protected override void Kinect_NewDepthFrame(Depth depthImage)
        {
            base.Kinect_NewDepthFrame(depthImage);

            if (Options.StoreDepth)
            {
                this.DepthImages.Enqueue(depthImage.DepthFrame);
                if (this.DepthImages.Count > this.Options.DepthMax)
                {
                    DepthImageFrame dead;
                    this.DepthImages.TryDequeue(out dead);
                }
            }
        }
    }
}
