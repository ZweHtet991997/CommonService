using CommonServices.Models.Enums.FTP;
using CommonServices.Models.FTPModel;

namespace CommonServices.Services.FTP_Service
{
    public interface IFluentFTPService
    {
        Task<bool> CheckDirectoryExistAsync(string directoryName);
        Task<bool> CreateDirectoryAsync(string directoryName);
        Task<EnumFTPResponse> DeleteFileAsync(FTPDeleteRequestModel model,bool isFileDelete);
        Task<EnumFTPResponse> FTPFileUploadServiceAsync(FTPRequestModel model);
        Task<string> FTPFileUploadAsync_V1(FTPModel model);
        Task UploadFileAsync(IFormFile file, string directoryName, string fileName);
    }
}