using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonServices.Models.DbContextModel
{
    [Table("Tbl_Project")]
    public class ProjectEntity
    {
        [Key]
        public int Id { get; set; }
        public int Database_Id { get; set; }
        public int FTP_Id { get; set; }
        public string ProjectName { get; set; } = null!;
    }
}
