using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GintySoft.KinectLib
{
    public abstract class KinectStorage : KinectBase
    {
        protected StorageOptions Options { get; set; }

        public KinectStorage(Kinect k, StorageOptions options)
            : base(k)
        {
            this.Options = options;
        }
    }
}
