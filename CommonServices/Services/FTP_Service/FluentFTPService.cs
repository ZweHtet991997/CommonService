using CommonServices.Models.Enums.FTP;
using CommonServices.Models.FTPModel;
using CommonServices.Resources;
using CommonServices.Service_Helpers.FTP;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;

namespace CommonServices.Services.FTP_Service
{
    public class FluentFTPService : IFluentFTPService
    {
        private AsyncFtpClient _ftp;
        private readonly ILogger<FluentFTPService> _logger;
        private readonly IFTPConfigService _ftpConfigService;

        public FluentFTPService(ILogger<FluentFTPService> logger, IFTPConfigService ftpConfigService)
        {
            _ftpConfigService = ftpConfigService;
            _logger = logger;
        }

        private AsyncFtpClient SetupFTPCredentials(FTPConfigModel model)
        {
            _ftp = new AsyncFtpClient
            {
                Host = model.Host,
                Credentials = new NetworkCredential(model.UserName, model.Password)
            };
            return _ftp;
        }

        public async Task<string> FTPFileUploadAsync_V1(FTPModel model)
        {
            try
            {
                List<FTPResponseModel> lstFileNames = new List<FTPResponseModel>();
                var fileNames = new List<string>();
                //Initializing FTP Credentials
                SetupFTPCredentials(model.FTPConfig);

                foreach (var file in model.Files)
                {
                    FTPResponseModel responseModel = new FTPResponseModel();
                    var fileName = RenameFileService.GetFileName(file);
                    responseModel.FileName = fileName;
                    lstFileNames.Add(responseModel);
                    if (!await CheckDirectoryExistAsync(model.DirectoryName))
                    {
                        if (!await CreateDirectoryAsync(model.DirectoryName))
                        {
                            return ResponseMessageResource.CreateFailed;
                        }
                    }
                    await UploadFileAsync(file, model.DirectoryName, fileName);
                }
                return JsonConvert.SerializeObject(lstFileNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in FTP File UploadService.");
                throw;
            }
        }

        public async Task<EnumFTPResponse> FTPFileUploadServiceAsync(FTPRequestModel model)
        {
            try
            {
                //Initializing FTP Credentials
                SetupFTPCredentials(model.FTPConfig);

                foreach (var fileProperty in model.FilePropertyModel)
                {
                    //var fileName = RenameFileService.GetFileName(fileProperty.File);

                    var fileName = fileProperty.FileName;

                    if (!await CheckDirectoryExistAsync(model.DirectoryName))
                    {
                        if (!await CreateDirectoryAsync(model.DirectoryName))
                        {
                            return EnumFTPResponse.DirectoryCreateFailed;
                        }
                    }

                    await UploadFileAsync(fileProperty.File, model.DirectoryName, fileName);
                }

                return EnumFTPResponse.FileUploaded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in FTP File UploadService.");
                return EnumFTPResponse.FileUploadFailed;
            }
        }

        //Check Directory Exist
        public async Task<bool> CheckDirectoryExistAsync(string directoryName)
        {
            try
            {
                await _ftp.Connect();
                return await _ftp.DirectoryExists(directoryName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error while Checking directory: {ex.InnerException}");
                throw;
            }
        }

        //Create New Directory
        public async Task<bool> CreateDirectoryAsync(string directoryName)
        {
            try
            {
                await _ftp.Connect();
                return await _ftp.CreateDirectory(directoryName);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null)
                {
                    _logger.LogInformation($"Create Directory Error: {ex.InnerException}");
                }

                _logger.LogInformation($"Create Directory Error: {ex.ToString()}");
                throw;
            }
        }

        //Upload File to Directory
        public async Task UploadFileAsync(IFormFile file, string directoryName, string fileName)
        {
            var tempFilePath = Path.GetTempFileName();
            _logger.LogInformation($"Upload File Temp File Path: {tempFilePath}");

            try
            {
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                await _ftp.Connect();
                _logger.LogInformation("Connected to FTP server.");

                var remoteFilePath = Path.Combine(directoryName, fileName).Replace("\\", "/");
                _logger.LogInformation($"Remote File Path: {remoteFilePath}");

                var success = await _ftp.UploadFile(tempFilePath, remoteFilePath);
                _logger.LogInformation("File Uploaded Successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Upload File Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogInformation($"Upload File Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                    _logger.LogInformation("Temporary file deleted.");
                }
            }
        }

        //Delete file from Directory
        public async Task<EnumFTPResponse> DeleteFileAsync(FTPDeleteRequestModel model, bool isFileDelete)
        {
            try
            {
                //Initializing FTP Credentials
                SetupFTPCredentials(model.FTPConfig);

                await _ftp.Connect();
                if (isFileDelete)
                {
                    await _ftp.DeleteFile(model.FilePath);
                }
                else
                {
                    await _ftp.DeleteDirectory(model.FilePath);
                }
                _logger.LogInformation("File Deleted Successfully!");
                return EnumFTPResponse.DeleteSuccess;
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null)
                {
                    _logger.LogInformation($"Delete File Exception: {ex.InnerException.Message}");
                }
                _logger.LogInformation(ex.Message);
                return EnumFTPResponse.DeleteFileFailed;
                throw;
            }
        }
    }
}
