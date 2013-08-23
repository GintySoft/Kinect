using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace GintySoft.KinectLib
{
    public class BitmapGenerator : KinectBase
    {
        public event BitmapReadyDelegate NewBitmapReady;
        public delegate void BitmapReadyDelegate(Bitmap b);

        private Bitmap m_bitmap = null;
        private Bitmap Bitmap {
            get
            {
                return m_bitmap;
            }
            set
            {
                this.m_bitmap = value;
            }
        }
        private object BitmapLock = new object();

        public BitmapGenerator(Kinect k)
            : base(k)
        {
           
        }

        protected override void Kinect_NewVideoFrame(VideoImage planerImage)
        {
            base.Kinect_NewVideoFrame(planerImage);

            //if (this.NewBitmapReady != null)
            //{
            //    lock (BitmapLock)
            //    {
            //        if (this.Bitmap == null)
            //        {
            //            Bitmap bmap = new Bitmap(
            //           planerImage.Image.Width,
            //           planerImage.Image.Height,
            //           PixelFormat.Format32bppRgb);
            //            this.Bitmap = bmap;
            //        }

            //        BitmapData bmapdata = this.Bitmap.LockBits(
            //         new Rectangle(0, 0, planerImage.Image.Width,
            //                             planerImage.Image.Height),
            //                             ImageLockMode.WriteOnly,
            //                             this.m_bitmap.PixelFormat);
            //        IntPtr ptr = bmapdata.Scan0; 
            //        Marshal.Copy(planerImage.Image.Bits, 0, ptr, planerImage.Image.Width * planerImage.Image.BytesPerPixel * planerImage.Image.Height);
            //        this.Bitmap.UnlockBits(bmapdata);
            //    }

            //    Task t = new Task(new Action(delegate
            //    {
            //        lock (BitmapLock)
            //        {
            //            NewBitmapReady(this.Bitmap);
            //        }
            //    }));
            //    t.Start();
            //}
        }
    }
}
