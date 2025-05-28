
namespace CommonServices.Services.Email_Service
{
    public interface ISendMailService
    {
        Task Send(EmailMetadata emailMetadata);
    }
}