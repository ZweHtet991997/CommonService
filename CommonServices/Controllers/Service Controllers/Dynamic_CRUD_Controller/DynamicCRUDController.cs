using Azure.Core;
using CommonServices.Services.Dynamic_CRUD_Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommonServices.Controllers.Service_Controllers.Dynamic_CRUD_Controller
{
    public class DynamicCRUDController : Controller
    {
        private ICRUDService _crudService;

        public DynamicCRUDController(ICRUDService crudService)
        {
            _crudService = crudService;
        }

        [HttpPost("service/create-data")]
        public async Task<IActionResult> CreateDataAsync([FromHeader] int projectId, [FromHeader] string tableName)
        {
            var isCreated = await _crudService.CreateDataAsync(projectId, tableName, Request.Body);
            return isCreated ? Ok("Success") : BadRequest("Failed to create data.Please try again.");
        }

        [HttpPut("service/update-data")]
        public async Task<IActionResult> UpdateDataAsync([FromHeader] int projectId, [FromHeader] string tableName)
        {
            var isUpdated = await _crudService.UpdateDataAsync(projectId, tableName, Request.Body);
            return isUpdated ? Ok("Success") : BadRequest("Failed to update data.Please try again.");
        }

        [HttpPost("service/get-datalist")]
        public async Task<IActionResult> GetDataListAsync([FromHeader] int projectId, [FromHeader] string tableName)
        {
            var dataList = await _crudService.GetDataListAsync(projectId, tableName);
            return Ok(dataList);
        }

        [HttpPost("service/filter-data")]
        public async Task<IActionResult> FilterDataAsync([FromHeader] int projectId, [FromHeader] string tableName)
        {
            var data = await _crudService.FilterDataAsync(projectId, tableName, Request.Body);
            return Ok(data);
        }

        [HttpDelete("service/delete-data")]
        public async Task<IActionResult> HardDeleteDataAsync([FromHeader] int projectId, [FromHeader] string tableName)
        {
            var isDeleted = await _crudService.HardDeleteDataAsync(projectId, tableName, Request.Body);
            return isDeleted ? Ok("Success") : BadRequest("Failed to delete data.Please try again.");
        }

        [HttpPut("service/softdelete-data")]
        public async Task<IActionResult> SoftDeleteDataAsync([FromHeader] int projecId, [FromHeader] string tableName)
        {
            var isDeleted = await _crudService.SoftDeleteDataAsync(projecId, tableName, Request.Body);
            return isDeleted ? Ok("Success") : BadRequest("Failed to delete data.Please try again");
        }
    }
}
