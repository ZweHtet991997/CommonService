

using CommonServices.Models.DbContextModel;
using CommonServices.Service_Helpers.Database;
using CommonServices.Service_Helpers.FTP;
using CommonServices.Services.Dynamic_CRUD_Service;
using CommonServices.Services.EncrypDecrypt_Service;
using CommonServices.Services.Firebase_Service;
using CommonServices.Services.FTP_Service;
using CommonServices.Services.Password_Policy_Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommonServices.Services.Common_Service
{
    public static class RegisteryService
    {
        public static IServiceCollection RegiteredServices(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            return services.AddCustomServices();
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            return services.AddTransient<ISendMailService, SendMailService>()
                .AddTransient<IVertificationService, VertificationService>()
                .AddTransient<IAesService, AesService>()
                .AddTransient<IDbConnectionService, DbConnectionService>()
                .AddTransient<IFTPConfigService, FTPConfigService>()
                .AddTransient<IFirebaseNotiService, FirebaseNotiService>()
                .AddScoped<IMailTypeMapper, MailTypeMapper>()
                .AddScoped<IFluentFTPService, FluentFTPService>()
                .AddScoped<ICRUDService, CRUDService>()
                .AddTransient<IAesService, AesService>();
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EFDbContext>(options =>
            options.UseSqlServer(DatabaseConfig.CommonServiceConnection())
            );
            return services;
        }
    }
}
