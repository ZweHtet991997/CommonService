
namespace CommonServices.Service_Helpers.Email
{
    public static class FluentMailService
    {
        public static void AddFluentEmailConfig(this IServiceCollection services)
        {
            var defaultFromEmail = EmailConfig.FromMail;
            var host = EmailConfig.Host;
            var port = EmailConfig.Port;
            var userName = EmailConfig.UserName;
            var password = EmailConfig.Password;
            services.AddFluentEmail(defaultFromEmail).AddSmtpSender(host, port, userName, password);
        }
    }
}
