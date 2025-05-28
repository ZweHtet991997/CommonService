namespace CommonServices.Configs
{
    public static class DatabaseConfig
    {
        public static string CommonServiceConnection()
        {
            return $"Data Source=SQL5111.site4now.net;Initial Catalog=db_a9a6b3_nkcommonservice;" +
                $"User Id=db_a9a6b3_nkcommonservice_admin;Password=NKsoftwarehouse*11";
        }
    }
}
