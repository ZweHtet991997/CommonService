
namespace CommonServices.Services.Account_Vertification_Service
{
    public interface IVertificationService
    {
        int GenerateAndStoreCode(string email);
        Enum ValidateCode(string email, int code);
    }
}