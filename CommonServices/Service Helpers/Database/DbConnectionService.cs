using CommonServices.Models.DbContextModel;
using Microsoft.EntityFrameworkCore;

namespace CommonServices.Service_Helpers.Database
{
    public class DbConnectionService : IDbConnectionService
    {
        private readonly EFDbContext _dbContext;

        public DbConnectionService(EFDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetDbConnectionService(int projectId)
        {
            var databaseId = await _dbContext.Tbl_Project
                .Where(x => x.Id == projectId)
                .Select(x => x.Database_Id).FirstOrDefaultAsync();

            var databaseInfo = await _dbContext.Tbl_DatabaseInfo
                .Where(x => x.Id == databaseId).FirstOrDefaultAsync();

            return $"Data Source={databaseInfo.Server};Initial Catalog={databaseInfo.Initial_Catalog};" +
                $"User Id={databaseInfo.User_Id};Password={databaseInfo.Password}";
        }
    }
}
