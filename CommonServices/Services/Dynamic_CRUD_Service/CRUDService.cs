using CommonServices.Service_Helpers.Database;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Text.Json;

namespace CommonServices.Services.Dynamic_CRUD_Service
{
    public class CRUDService : ICRUDService
    {
        private readonly IDbConnectionService _dbConnectionService;
        private string _connectionString;

        public CRUDService(IDbConnectionService dbConnectionService)
        {
            _dbConnectionService = dbConnectionService;
        }

        #region CreateData
        public async Task<bool> CreateDataAsync(int projectId, string tableName, Stream jsonBody)
        {
            try
            {
                using var reader = new StreamReader(jsonBody);
                var body = await reader.ReadToEndAsync();

                // Parse JSON body into a dictionary
                var jsonDoc = JsonDocument.Parse(body);
                var jsonData = jsonDoc.RootElement;

                var columnNames = string.Join(", ", jsonData.EnumerateObject().Select(p => p.Name));
                var paramNames = string.Join(", ", jsonData.EnumerateObject().Select(p => $"@{p.Name}"));

                var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";
                var parameters = new DynamicParameters();

                // Populate DynamicParameters with data from JSON
                foreach (var property in jsonData.EnumerateObject())
                {
                    var value = property.Value;
                    parameters.Add($"@{property.Name}", value.ValueKind switch
                    {
                        JsonValueKind.String => value.GetString(),
                        JsonValueKind.Number when value.TryGetInt32(out int intValue) => intValue,
                        JsonValueKind.Number when value.TryGetInt64(out long longValue) => longValue,
                        JsonValueKind.Number when value.TryGetDecimal(out decimal decimalValue) => decimalValue,
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        JsonValueKind.Null => null,
                        _ => throw new InvalidOperationException("Unsupported JSON data type")
                    });
                }
                _connectionString = await _dbConnectionService.GetDbConnectionService(projectId);
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(sql, parameters);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message.ToString());
            }
        }
        #endregion

        #region UpdateDate
        public async Task<bool> UpdateDataAsync(int projectId, string tableName, Stream jsonBody)
        {
            try
            {
                using var reader = new StreamReader(jsonBody);
                var body = await reader.ReadToEndAsync();

                // Parse JSON body into a dictionary
                var jsonDoc = JsonDocument.Parse(body);
                var jsonData = jsonDoc.RootElement;

                // Identify the identifier dynamically (e.g., UserId, Category_Id, etc.)
                var identifier = jsonData.EnumerateObject().FirstOrDefault(p => p.Name.
                EndsWith("Id", StringComparison.OrdinalIgnoreCase) || p.Name.Contains("_Id"));

                if (identifier.Value.ValueKind == JsonValueKind.Undefined)
                    return false;

                var identifierName = identifier.Name;//Get identifier name only

                //Construct SET clause, exculding the identifier
                var setClause = string.Join(", ", jsonData.EnumerateObject()
                    .Where(p => p.Name != identifierName)
                    .Select(p => $"{p.Name} = @{p.Name}"));

                var sql = $"UPDATE {tableName} SET {setClause} WHERE {identifierName} = @{identifierName}";
                var parameters = new DynamicParameters();

                // Populate DynamicParameters with data from JSON
                foreach (var property in jsonData.EnumerateObject())
                {
                    var value = property.Value;
                    parameters.Add($"@{property.Name}", value.ValueKind switch
                    {
                        JsonValueKind.String => value.GetString(),
                        JsonValueKind.Number when value.TryGetInt32(out int intValue) => intValue,
                        JsonValueKind.Number when value.TryGetInt64(out long longValue) => longValue,
                        JsonValueKind.Number when value.TryGetDecimal(out decimal decimalValue) => decimalValue,
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        JsonValueKind.Null => null,
                        _ => throw new InvalidOperationException("Unsupported JSON data type")
                    });
                }

                // Get the connection string and execute the query
                _connectionString = await _dbConnectionService.GetDbConnectionService(projectId);
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.ExecuteAsync(sql, parameters);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message.ToString());
            }
        }
        #endregion

        #region GetDataList
        public async Task<IEnumerable<dynamic>> GetDataListAsync(int projectId, string tableName)
        {
            try
            {
                _connectionString = await _dbConnectionService.GetDbConnectionService(projectId);
                using (var connection = new SqlConnection(_connectionString))
                {
                    var queryBuilder = new StringBuilder($"SELECT * FROM {tableName} WHERE 1=1");
                    var dataList = await connection.QueryAsync<dynamic>(queryBuilder.ToString());
                    return dataList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message.ToString());
            }
        }
        #endregion

        #region FilterData

        public async Task<IEnumerable<dynamic>> FilterDataAsync(int projectId, string tableName, Stream jsonBody)
        {
            try
            {
                using var reader = new StreamReader(jsonBody);
                var body = await reader.ReadToEndAsync();

                var jsonDoc = JsonDocument.Parse(body);
                var jsonData = jsonDoc.RootElement;

                // Initialize the SQL query with the SELECT statement
                var sqlBuilder = new StringBuilder($"SELECT * FROM {tableName} WHERE ");
                var parameters = new DynamicParameters();
                bool hasConditions = false;

                // Dynamically build WHERE clause based on JSON properties
                foreach (var property in jsonData.EnumerateObject())
                {
                    var value = property.Value;
                    var parameterName = $"@{property.Name}";

                    // Append condition for each property
                    sqlBuilder.Append($"{property.Name} = {parameterName} AND ");
                    hasConditions = true;

                    // Add parameter based on the value type
                    parameters.Add(parameterName, value.ValueKind switch
                    {
                        JsonValueKind.String => value.GetString(),
                        JsonValueKind.Number when value.TryGetInt32(out int intValue) => intValue,
                        JsonValueKind.Number when value.TryGetInt64(out long longValue) => longValue,
                        JsonValueKind.Number when value.TryGetDecimal(out decimal decimalValue) => decimalValue,
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        JsonValueKind.Null => null,
                        _ => throw new InvalidOperationException("Unsupported JSON data type")
                    });
                }

                // Remove the trailing " AND " from the SQL query if conditions were added
                if (hasConditions)
                {
                    sqlBuilder.Length -= 5;
                }
                else
                {
                    // If no conditions, remove WHERE clause
                    sqlBuilder.Length -= 7;
                }

                //Get the connection string and execute the query
                _connectionString = await _dbConnectionService.GetDbConnectionService(projectId);
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync(sqlBuilder.ToString(), parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message.ToString());
            }
        }

        #endregion

        #region HardDelete

        public async Task<bool> HardDeleteDataAsync(int projectId, string tableName, Stream jsonBody)
        {
            try
            {
                using var reader = new StreamReader(jsonBody);
                var body = await reader.ReadToEndAsync();

                var jsonDoc = JsonDocument.Parse(body);
                var jsonData = jsonDoc.RootElement;

                // Find the identifier dynamically (e.g., UserId, Category_Id, etc.)
                var identifier = jsonData.EnumerateObject().FirstOrDefault(p => p.Name
                .EndsWith("Id", StringComparison.OrdinalIgnoreCase) || p.Name.Contains("_Id"));

                if (identifier.Value.ValueKind == JsonValueKind.Undefined)
                    return false;

                var identifierName = identifier.Name;//get identifier name only

                object identifierValue = identifier.Value.ValueKind == JsonValueKind.Number
                ? identifier.Value.GetInt32()   //assume that the parameter Id is integer
                : identifier.Value.GetString(); //assume that the parameter Id is string

                // Construct the SQL delete statement
                var sql = $"DELETE FROM {tableName} WHERE {identifierName} = @{identifierName}";
                var parameters = new DynamicParameters();
                parameters.Add($"@{identifierName}", identifierValue);

                // Get the connection string and execute the query
                _connectionString = await _dbConnectionService.GetDbConnectionService(projectId);
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.ExecuteAsync(sql, parameters);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message.ToString());
            }
        }

        #endregion

        public async Task<bool> SoftDeleteDataAsync(int projectId, string tableName, Stream jsonBody)
        {
            try
            {
                using var reader = new StreamReader(jsonBody);
                var body = await reader.ReadToEndAsync();

                var jsonDoc = JsonDocument.Parse(body);
                var jsonData = jsonDoc.RootElement;

                // Find the identifier dynamically (e.g., UserId, Category_Id, etc.)
                var identifier = jsonData.EnumerateObject().FirstOrDefault(p => p.Name
                    .EndsWith("Id", StringComparison.OrdinalIgnoreCase) || p.Name.Contains("_Id"));

                if (identifier.Value.ValueKind == JsonValueKind.Undefined)
                    return false;

                var identifierName = identifier.Name; // Get identifier name only

                object identifierValue = identifier.Value.ValueKind == JsonValueKind.Number
                    ? identifier.Value.GetInt32()   // Assume that the parameter Id is integer
                    : identifier.Value.GetString(); // Assume that the parameter Id is string

                // Construct the SQL update statement for soft delete
                var sql = $"UPDATE {tableName} SET IsDeleted = 1 WHERE {identifierName} = @{identifierName}";
                var parameters = new DynamicParameters();
                parameters.Add($"@{identifierName}", identifierValue);

                // Get the connection string and execute the query
                _connectionString = await _dbConnectionService.GetDbConnectionService(projectId);
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.ExecuteAsync(sql, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

    }
}
