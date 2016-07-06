using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        // TODO: Add your service operations here
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ListFoldersInRootOfContainer?container={container}", Method = "GET", RequestFormat= WebMessageFormat.Json, ResponseFormat= WebMessageFormat.Json)]
        List<Blob> ListFoldersInRootOfContainer(string container);
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ListFilesInSpecificFolder?container={container}&folderName={folderName}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Blob> ListFilesInSpecificFolder(string container, string folderName);
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/UploadFileOnSpecificFolder?container={container}&filePath={filePath}&folderNameInContainer={folderNameInContainer}&FileOrFolderNameInContainer={FileOrFolderNameInContainer}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool UploadFileOnSpecificFolder(string container, string filePath, string folderNameInContainer, string FileOrFolderNameInContainer);
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/DownloadSpecificFile?container={container}&folder={folder}&fileName={fileName}&downloadFilePath={downloadFilePath}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Blob DownloadSpecificFile(string container, string folder, string fileName, string downloadFilePath);
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ZipBlobDirectoryToArchives?container={container}&folderNameInContainer={folderNameInContainer}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Blob ZipBlobDirectoryToArchives(string container, string folderNameInContainer);
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ZipLocalDirectoryToContainer?container={container}&directoryPath={directoryPath}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void ZipLocalDirectoryToContainer(string container, string directoryPath);

    }

}
