namespace CommonServices.Models.DbContextModel
{
    public class FTPEntity
    {
        public int Id { get; set; }
        public string Host { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
