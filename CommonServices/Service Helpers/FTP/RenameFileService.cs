namespace CommonServices.Service_Helpers.FTP
{
    public static class RenameFileService
    {
        public static string GetFileName(this IFormFile file)
        {
            TimeZoneInfo myanmarTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time");
            DateTime localDateTime = DateTime.Now;
            DateTime myanmarDateTime = TimeZoneInfo.ConvertTime(
                localDateTime,
                TimeZoneInfo.Local,
                myanmarTimeZone
            );
            long unixTimeMilliseconds = new DateTimeOffset(myanmarDateTime).ToUnixTimeMilliseconds();
            return $"{unixTimeMilliseconds}_" + file.FileName;
        }
    }
}
