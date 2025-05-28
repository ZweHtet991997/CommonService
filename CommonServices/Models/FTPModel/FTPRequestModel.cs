namespace CommonServices.Models.FTPModel
{
    public class FTPRequestModel
    {
        public int ProjectId { get; set; }
        public string DirectoryName { get; set; } = null!;
        public List<FilePropertyModel> FilePropertyModel { get; set; } = null!;
        public FTPConfigModel FTPConfig { get; set; }
    }

    public class FTPDeleteRequestModel
    {
        public int ProjectId { get; set; }
        public FTPConfigModel FTPConfig { get; set; }
        public string FilePath { get; set; }
    }

    public class FilePropertyModel
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }

    public class FTPModel
    {
        public int ProjectId { get; set; }
        public string DirectoryName { get; set; } = null!;
        public List<IFormFile> Files { get; set; }
        public FTPConfigModel FTPConfig { get; set; }
    }

    public class FTPResponseModel
    {
        public string FileName { get; set; }
    }
}
