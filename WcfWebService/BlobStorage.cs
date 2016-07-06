using Ionic.Zip;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WcfWebService
{
    //Used for blob operations
    public class BlobStorage
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
        //Upload one blob
        public string PutBlob(string blobName, string filePath)
        {
            var blockBlob = Container.GetBlockBlobReference(blobName);
            blockBlob.UploadFromFile(filePath, System.IO.FileMode.Open);
            return blockBlob.Uri.ToString();
        }
        //Download one blob
        public string DownloadBlob(string blobName, string downloadFilePath)
        {
            var blockBlob = Container.GetBlockBlobReference(blobName);
            blockBlob.DownloadToFile(downloadFilePath, System.IO.FileMode.OpenOrCreate);
            return blockBlob.Uri.ToString();
        }
        //Download many blobs in one folder of container to local storage
        public void DownloadToLocalStorage(LocalResource mystorage, string folderNameInContainer, string fileRootPath)
        {
            string blobPrefix = folderNameInContainer + "/";
            bool useFlatBlobListing = false;
            var blobs = Container.ListBlobs(blobPrefix, useFlatBlobListing, BlobListingDetails.None);
            var filesOrFoders = blobs.ToList();
            //Create direcotry in local storage
            Directory.CreateDirectory(fileRootPath);
            string fileOrFolderName;
            //If it is file, download it. If it is directory, use this function again and add this folder name after original folder name
            foreach(var fileOrFolder in filesOrFoders)
            {
                if(fileOrFolder.GetType() != typeof(CloudBlobDirectory))
                {
                    fileOrFolderName = Path.GetFileName(fileOrFolder.Uri.ToString());
                    DownloadBlob(Path.Combine(folderNameInContainer, fileOrFolderName), Path.Combine(fileRootPath, fileOrFolderName));
                }
                else if(fileOrFolder.GetType() == typeof(CloudBlobDirectory))
                {
                    fileOrFolderName = fileOrFolder.Uri.ToString();
                    fileOrFolderName = fileOrFolderName.Substring(0, fileOrFolderName.Length - 1);
                    fileOrFolderName = fileOrFolderName.Substring(fileOrFolderName.LastIndexOf("/") + 1);
                    //Call this function again
                    DownloadToLocalStorage(mystorage, folderNameInContainer + "/" + fileOrFolderName, Path.Combine(fileRootPath, fileOrFolderName));
                }

            }
        }

        //Upload folder to the container, if it has subfolders, upload these subfolders too.
        public void UploadFolder(string container, string directoryPath, string folderName)
        {
            var folder = new DirectoryInfo(directoryPath);
            var files = folder.GetFiles();
            foreach (var fileInfo in files)
            {
                var blobName = folderName + "/" + fileInfo.Name;
                PutBlob(blobName, fileInfo.FullName);
            }
            var subFolders = folder.GetDirectories();
            foreach (var directoryInfo in subFolders)
            {
                UploadFolder(container, directoryInfo.FullName, folderName + "/" + directoryInfo.Name);
            }
        }
        //To unzip one zip file on local disk
        public string UnZipFiles(string filePath)
        {
            ZipFile zipFile = ZipFile.Read(filePath);
            string outputDirectory = Path.GetDirectoryName(filePath) + Path.GetFileNameWithoutExtension(filePath) + "ZipFile";
            Directory.CreateDirectory(outputDirectory);
            zipFile.ExtractAll(outputDirectory, ExtractExistingFileAction.OverwriteSilently);
            zipFile.Dispose();
            return outputDirectory;
        }

    }
}