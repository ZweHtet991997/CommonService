namespace CommonServices.Configs
{
    public static class EmailConfig
    {
        private static string DefaultFromEmail()
        {
            return "zwehtet.nksoftwarehouse@gmail.com";
        }

        private static string EmailHost()
        {
            return "smtp.gmail.com";
        }

        private static int EmailPort()
        {
            return 587;
        }

        private static string MailUserName()
        {
            return "zwehtet.nksoftwarehouse@gmail.com";
        }

        private static string MailPassword()
        {
            return "riftwqktrxudiiky";
        }

        public static string FromMail => DefaultFromEmail();
        public static string Host => EmailHost();
        public static int Port => EmailPort();
        public static string UserName => MailUserName();
        public static string Password => MailPassword();
    }
}
