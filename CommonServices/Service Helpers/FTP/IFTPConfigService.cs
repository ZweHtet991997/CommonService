using CommonServices.Models.FTPModel;

namespace CommonServices.Service_Helpers.FTP
{
    public interface IFTPConfigService
    {
        Task<FTPConfigModel> GetFtpConfiguration(int projectId);
    }
}