using CommonServices.Models.Enums.FTP;
using CommonServices.Models.FTPModel;
using CommonServices.Service_Helpers.FTP;
using CommonServices.Services.FTP_Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommonServices.Controllers.Service_Controllers.FTP_Controller
{
    public class FTPServiceController : Controller
    {
        private readonly IFluentFTPService _ftpService;
        private readonly IFTPConfigService _ftpCofigService;

        public FTPServiceController(IFluentFTPService ftpService, IFTPConfigService ftpCofigService)
        {
            _ftpService = ftpService;
            _ftpCofigService = ftpCofigService;
        }

        [HttpPost("service/ftp/v1")]
        public async Task<IActionResult> UploadFile_V1([FromForm] FTPModel model)
        {
            if (model.Files.Count > 0)
            {
                #region GetFTPConfigByProjectId
                var ftpCredentials = await _ftpCofigService.GetFtpConfiguration(model.ProjectId);
                model.FTPConfig = ftpCredentials;
                #endregion

                var dataResult = await _ftpService.FTPFileUploadAsync_V1(model);
                var responseModel = JsonConvert.DeserializeObject<List<FTPResponseModel>>(dataResult);
                return Ok(dataResult);

            }
            return BadRequest("No File found");
        }

        [HttpPost("service/ftp")]
        public async Task<IActionResult> UploadFile([FromForm] FTPRequestModel model)
        {
            if (model.FilePropertyModel.Count > 0)
            {
                //Get FTP Credentials
                var ftpCredentials = await _ftpCofigService.GetFtpConfiguration(model.ProjectId);
                model.FTPConfig = ftpCredentials;

                var dataResult = await _ftpService.FTPFileUploadServiceAsync(model);

                switch (dataResult)
                {
                    case EnumFTPResponse.FileUploaded:
                        return Ok("Success");

                    case EnumFTPResponse.DirecctoryCheckFailed:
                        return StatusCode(StatusCodes.Status400BadRequest, "Fail in checking directory exist");
                    case EnumFTPResponse.DirectoryCreateFailed:
                        return BadRequest("Failed to create new directory");

                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error.");
                }
            }
            return BadRequest("No File Found");
        }

        [HttpDelete("service/ftp/file")]
        public async Task<IActionResult> DeleteFile([FromBody] FTPDeleteRequestModel model)
        {
            //Get FTP Credentials
            var ftpCredentials = await _ftpCofigService.GetFtpConfiguration(model.ProjectId);
            model.FTPConfig = ftpCredentials;
            var dataResult = await _ftpService.DeleteFileAsync(model, true);
            if (dataResult == EnumFTPResponse.DeleteSuccess)
            {
                return Ok();
            }
            return BadRequest("Failed to Delete");

        }

        [HttpDelete("service/ftp/dir")]
        public async Task<IActionResult> DeleteFolder([FromBody] FTPDeleteRequestModel model)
        {
            //Get FTP Credentials
            var ftpCredentials = await _ftpCofigService.GetFtpConfiguration(model.ProjectId);
            model.FTPConfig = ftpCredentials;
            var dataResult = await _ftpService.DeleteFileAsync(model, false);
            if (dataResult == EnumFTPResponse.DeleteSuccess)
            {
                return Ok();
            }
            return BadRequest("Failed to Delete");

        }
    }
}
