using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GintySoft.KinectLib
{
    public class StorageOptions
    {
        public bool StoreColor { get; set; }
        public bool StoreSkeleton { get; set; }
        public bool StoreDepth { get; set; }
        public long ColorMax { get; set; }
        public long DepthMax { get; set; }
        public long SkeltonMax { get; set; }

        public StorageOptions()
        {
            this.StoreColor = false;
            this.StoreDepth = false;
            this.StoreSkeleton = false;
            this.ColorMax = 0;
            this.DepthMax = 0;
            this.SkeltonMax = 0;
        }
    }
}
