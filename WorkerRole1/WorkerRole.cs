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
using Microsoft.WindowsAzure.Storage;
using Ionic.Zip;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole1 is running"); 
            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }
        
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                /*
                 * To backup all files to folder backups every 60s. And save no more than 5 backup files. 
                 * If you want to change the blob container, you can change the "blobcontainer" to your blob container name.
                 */
                var blobStorage = new BlobStorage("blobcontainer");
                //Use local storage
                LocalResource myStorage = RoleEnvironment.GetLocalResource("Backup");
                string filePath = Path.Combine(myStorage.RootPath, "backup");
                Directory.CreateDirectory(filePath);

                //Download all files to local storage
                blobStorage.DownloadToLocalStorage(myStorage, "", filePath);
                //Zip the directory
                ZipFile zipFile = new ZipFile();
                string zipPath = filePath + ".zip";
                zipFile.AddDirectory(filePath);
                zipFile.Save(zipPath);
                zipFile.Dispose();
                //Put it back to archives folder of container
                string uploadPath = Path.Combine("backups", Path.GetFileNameWithoutExtension(zipPath) + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".zip");
                blobStorage.PutBlob(uploadPath, zipPath);

                var blobs = blobStorage.Container.ListBlobs("backups/", false, BlobListingDetails.None);

                //No more than 5 backup files
                if(blobs.Count() > 5)
                {
                    var deleteBlob = blobs.OrderBy(b => b.Uri.ToString()).First();
                    var blob = blobStorage.Container.GetBlockBlobReference("backups/" + Path.GetFileName(deleteBlob.Uri.ToString()));
                    blob.Delete();
                }

                Trace.TraceInformation("Working");
                await Task.Delay(60000);
            }
        }
    }
}
