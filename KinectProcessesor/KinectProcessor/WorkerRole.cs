using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using KinectLib;

namespace KinectProcessor
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            Task.Factory.StartNew(new Action(processImages));
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
        private void processImages()
        {
            AzureWrapper imageazure;
            try
            {
                imageazure = new AzureWrapper(AzureStorage.VIDEO_CONTAINER_NAME);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            while (true)
            {
                try
                {
                    CloudQueueMessage message = imageazure.Queue.GetMessage();
                    string id = message.AsString;
                    IListBlobItem blobitem = imageazure.BlobContainer.ListBlobs().Where(b => b.Uri.ToString().Contains(id)).First();
                    CloudBlob blob = imageazure.BlobContainer.GetBlobReference(blobitem.Uri.ToString());
                    byte[] package = blob.DownloadByteArray();
                    Compression<ImageFrameSerialized> comp = new Compression<ImageFrameSerialized>();
                    ImageFrameSerialized frame = comp.GZipUncompress(package);
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }
        }
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
