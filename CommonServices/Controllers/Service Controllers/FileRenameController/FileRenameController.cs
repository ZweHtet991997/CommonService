using CommonServices.Service_Helpers.FTP;
using Microsoft.AspNetCore.Mvc;

namespace CommonServices.Controllers.Service_Controllers.FileRenameController
{
    public class FileRenameController : Controller
    {
        [HttpPost("service/rename")]
        public Task<IActionResult> FileRename([FromForm] IFormFile file)
        {
            return Task.FromResult<IActionResult>(Ok(RenameFileService.GetFileName(file)));
        }
    }
}
