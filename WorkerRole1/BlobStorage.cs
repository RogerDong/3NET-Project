using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    class BlobStorage
    {
        private CloudStorageAccount StorageAccount { get; set; }
        private CloudBlobClient BlobClient { get; set; }
        public CloudBlobContainer Container { get; set; }
        public BlobStorage(string containerName)
        {
            StorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            BlobClient = StorageAccount.CreateCloudBlobClient();
            Container = BlobClient.GetContainerReference(containerName);
        }
        //Download one blob
        public void DownloadBlob(string blobName, string downloadFilePath)
        {
            var blockBlob = Container.GetBlockBlobReference(blobName);
            blockBlob.DownloadToFile(downloadFilePath, System.IO.FileMode.OpenOrCreate);
        }

        //Upload one blob
        public void PutBlob(string blobName, string filePath)
        {
            var blockBlob = Container.GetBlockBlobReference(blobName);
            blockBlob.UploadFromFile(filePath, System.IO.FileMode.Open);
        }
        //Download many blobs in one folder of container to local storage
        public void DownloadToLocalStorage(LocalResource mystorage, string folderNameInContainer, string fileRootPath)
        {
            string blobPrefix = folderNameInContainer + "/";
            if(blobPrefix == "/")
            {
                blobPrefix = null;
            }
            bool useFlatBlobListing = false;
            var blobs = Container.ListBlobs(blobPrefix, useFlatBlobListing, BlobListingDetails.None);
            var filesOrFoders = blobs.ToList();
            //Create direcotry in local storage
            Directory.CreateDirectory(fileRootPath);
            string fileOrFolderName;
            //If it is file, download it. If it is directory, use this function again and add this folder name after original folder name
            foreach (var fileOrFolder in filesOrFoders)
            {
                if (fileOrFolder.GetType() != typeof(CloudBlobDirectory))
                {
                    fileOrFolderName = Path.GetFileName(fileOrFolder.Uri.ToString());

                    DownloadBlob(Path.Combine(folderNameInContainer, fileOrFolderName), Path.Combine(fileRootPath, fileOrFolderName));
                }
                else if (fileOrFolder.GetType() == typeof(CloudBlobDirectory))
                {
                    
                    fileOrFolderName = fileOrFolder.Uri.ToString();
                    fileOrFolderName = fileOrFolderName.Substring(0, fileOrFolderName.Length - 1);
                    fileOrFolderName = fileOrFolderName.Substring(fileOrFolderName.LastIndexOf("/") + 1);
                    if(folderNameInContainer == "")
                    {
                        if (fileOrFolderName != "backups")
                        {
                            DownloadToLocalStorage(mystorage, fileOrFolderName, Path.Combine(fileRootPath, fileOrFolderName));
                        }
                    }
                    else
                    {
                        DownloadToLocalStorage(mystorage, folderNameInContainer + "/" + fileOrFolderName, Path.Combine(fileRootPath, fileOrFolderName));
                    }
                }
            }
        }
    }
}
