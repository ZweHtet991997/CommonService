using Microsoft.EntityFrameworkCore;

namespace CommonServices.Models.DbContextModel
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ProjectEntity> Tbl_Project { get; set; }
        public DbSet<DatabaseInfoEntity> Tbl_DatabaseInfo { get; set; }
        public DbSet<FTPEntity> Tbl_FtpInfo { get; set; }
    }
}
