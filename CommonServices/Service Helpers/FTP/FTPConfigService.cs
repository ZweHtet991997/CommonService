using CommonServices.Models.DbContextModel;
using CommonServices.Models.FTPModel;
using Microsoft.EntityFrameworkCore;

namespace CommonServices.Service_Helpers.FTP
{
    public class FTPConfigService : IFTPConfigService
    {
        private readonly EFDbContext _dbContext;

        public FTPConfigService(EFDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FTPConfigModel> GetFtpConfiguration(int projectId)
        {
            var ftpId = await _dbContext.Tbl_Project
                .Where(x => x.Id == projectId)
                .Select(x => x.FTP_Id).FirstOrDefaultAsync();

            var ftpInfo = await _dbContext.Tbl_FtpInfo
                .Where(x => x.Id == ftpId).FirstOrDefaultAsync();

            return new FTPConfigModel
            {
                Host = ftpInfo.Host,
                UserName = ftpInfo.UserName,
                Password = ftpInfo.Password,
            };
        }
    }
}
