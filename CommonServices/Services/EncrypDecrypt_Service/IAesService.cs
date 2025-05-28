namespace CommonServices.Services.EncrypDecrypt_Service
{
    public interface IAesService
    {
        string DecryptString(string encryptedText);
        string EncryptString(string raw);
    }
}