using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public class AllFrames
    {
        public VideoImage Image { get; set; }
        public Skel Skeleton { get; set; }
        public Depth Depth { get; set; }
    }
}
