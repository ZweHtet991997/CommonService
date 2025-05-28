
namespace CommonServices.Services.Firebase_Service
{
    public interface IFirebaseNotiService
    {
        Task<string> SendNotiAsync(string deviceToken, string title, string message);
    }
}