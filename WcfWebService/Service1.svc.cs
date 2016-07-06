using Ionic.Zip;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
namespace WcfWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        //Zip an directory from container and zip it, then put it in arhives file. return the blob info
        public Blob ZipBlobDirectoryToArchives(string container, string folderNameInContainer)
        {
            var blobStorage = new BlobStorage(container);
            //Use local storage
            LocalResource myStorage = RoleEnvironment.GetLocalResource("LocalStorage1");

            string filePath = Path.Combine(myStorage.RootPath, folderNameInContainer);
            //Download all files to local storage
            blobStorage.DownloadToLocalStorage(myStorage, folderNameInContainer, filePath);
            //Zip the directory
            ZipFile zipFile = new ZipFile();
            string zipPath = filePath + ".zip";
            zipFile.AddDirectory(filePath);
            zipFile.Save(zipPath);
            zipFile.Dispose();
            //Put it back to archives folder of container
            string uploadPath =  Path.Combine("archives", Path.GetFileName(zipPath));
            var blob = new Blob();
            blob.Name = Path.GetFileName(zipPath);
            blob.Uri =  blobStorage.PutBlob(uploadPath, zipPath);
            return blob;
        }
        //Download a specific file, regardless of zip file or not. return the blob info
        public Blob DownloadSpecificFile(string container, string folder, string fileName, string downloadFilePath)
        {
            BlobStorage blobStorage = new BlobStorage(container);
            var blob = new Blob();
            blob.Name = fileName;
            blob.Uri = blobStorage.DownloadBlob(folder + "/" + fileName, downloadFilePath);
            return blob;
        }
        //Upload file to a specific folder. Check whether it is zip file. If it is, unzip it and upload the whole folder.
        public bool UploadFileOnSpecificFolder(string container, string filePath, string folderNameInContainer, string FileOrFolderNameInContainer)
        {
            BlobStorage blobStorage = new BlobStorage(container);
            if (string.IsNullOrEmpty(folderNameInContainer) || string.IsNullOrWhiteSpace(folderNameInContainer))
            { 
                return false;
            }
            if (System.IO.Path.GetExtension(filePath) == ".zip")
            {
                var unzipedFolderPath = blobStorage.UnZipFiles(filePath);
                blobStorage.UploadFolder(container, unzipedFolderPath, folderNameInContainer + "/" + FileOrFolderNameInContainer);
                Directory.Delete(unzipedFolderPath, true);
            }
            else
            {
                blobStorage.PutBlob(folderNameInContainer + "/" + FileOrFolderNameInContainer, filePath);
            }
            return true;

        }
        

        //Zip a local directory to arhives folder of the container
        public void ZipLocalDirectoryToContainer(string container, string directoryPath)
        {   
            ZipFile zf = new ZipFile();
            zf.AddDirectory(directoryPath);
            var zipFilePath = Path.GetDirectoryName(directoryPath) + Path.GetFileName(directoryPath) + ".zip";
            zf.Save(zipFilePath);
            zf.Dispose();
            var blobName =  "archives/" + Path.GetFileName(directoryPath) + ".zip";
            var blobStorage = new BlobStorage(container);
            blobStorage.PutBlob(blobName, zipFilePath);
        }
        //List all files in a specific folder(for example, folder name:  a/b/c). return the list of blob info.
        public List<Blob> ListFilesInSpecificFolder(string container, string folderName)
        {
            BlobStorage blobStorage = new BlobStorage(container);
            //Add prefix before listing all files
            string blobPrefix = folderName + "/";
            if (string.IsNullOrEmpty(folderName) || string.IsNullOrWhiteSpace(folderName))
            {
                blobPrefix = null;
            }
            //Not to list the files in subfolder
            bool useFlatBlobListing = false;
            var blobs = blobStorage.Container.ListBlobs(blobPrefix, useFlatBlobListing, BlobListingDetails.None);
            //To get all files
            var files = blobs.Where(b => b as CloudBlobDirectory == null).ToList();

            var list = new List<Blob>();
            foreach (var file in files)
            {
                var blob = new Blob();
                //To get blob info
                blob.Name = Path.GetFileName(file.Uri.ToString());
                blob.Uri = file.Uri.ToString();
                list.Add(blob);
            }
            return list;
        }
        //List all foders in the root of the container
        public List<Blob> ListFoldersInRootOfContainer(string container)
        {
            BlobStorage blobStorage = new BlobStorage(container);
            string blobPrefix = null;
            //Not to list the files in subfolder
            bool useFlatBlobListing = false;  
            var blobs = blobStorage.Container.ListBlobs(blobPrefix, useFlatBlobListing, BlobListingDetails.None);
            //To get all the directory
            var folders = blobs.Where(b => b as CloudBlobDirectory != null).ToList();
            var list = new List<Blob>();
            string folderName;
            foreach (var folder in folders)
            {
                var blob = new Blob();
                //Get the folder name
                folderName = folder.Uri.ToString();
                folderName = folderName.Substring(0, folderName.Length - 1);
                folderName = folderName.Substring(folderName.LastIndexOf("/") + 1);
                //Assign the folder name and Uri
                blob.Name = folderName;
                blob.Uri = folder.Uri.ToString();
                list.Add(blob);
            }
            //Return the list
            return list;
        }

    }
}
