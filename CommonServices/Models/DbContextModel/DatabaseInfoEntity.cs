using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonServices.Models.DbContextModel
{
    [Table("Tbl_DatabaseInfo")]
    public class DatabaseInfoEntity
    {
        [Key]
        public int Id { get; set; }
        public string Server { get; set; } = null!;
        public string Initial_Catalog { get; set; } = null!;
        public string User_Id { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
