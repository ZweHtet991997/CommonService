
namespace CommonServices.Services.Dynamic_CRUD_Service
{
    public interface ICRUDService
    {
        Task<bool> CreateDataAsync(int projectId, string tableName, Stream jsonBody);
        Task<IEnumerable<dynamic>> FilterDataAsync(int projectId, string tableName, Stream jsonBody);
        Task<IEnumerable<dynamic>> GetDataListAsync(int projectId, string tableName);
        Task<bool> HardDeleteDataAsync(int projectId, string tableName, Stream jsonBody);
        Task<bool> SoftDeleteDataAsync(int projectId, string tableName, Stream jsonBody);
        Task<bool> UpdateDataAsync(int projectId, string tableName, Stream jsonBody);
    }
}