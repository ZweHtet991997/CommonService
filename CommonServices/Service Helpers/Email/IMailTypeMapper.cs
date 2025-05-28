
namespace CommonServices.Service_Helpers.Email
{
    public interface IMailTypeMapper
    {
        string GetMailType(MailRequestModel model);
    }
}