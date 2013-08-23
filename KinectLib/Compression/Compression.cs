using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.IO;

namespace GintySoft.KinectLib
{
    public class Compression<T>
    {
        public Compression()
        {
     
        }

        public byte[] GzipCompress(T compressMe)
        {
            MemoryStream sourcemem = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(sourcemem, compressMe);
            MemoryStream targetmem = new MemoryStream();
            GZipStream gzipper = new GZipStream(targetmem, CompressionMode.Compress);
            sourcemem.WriteTo(gzipper);
            return targetmem.ToArray();
        }
        public T GZipUncompress(byte[] bytesToUncompress)
        {
            MemoryStream sourceMem = new MemoryStream(bytesToUncompress);
            BinaryFormatter bf = new BinaryFormatter();
            T obj = (T)bf.Deserialize(sourceMem);
            return obj;
        }
    }
}
