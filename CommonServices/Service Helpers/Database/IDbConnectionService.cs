
namespace CommonServices.Service_Helpers.Database
{
    public interface IDbConnectionService
    {
        Task<string> GetDbConnectionService(int projectId);
    }
}