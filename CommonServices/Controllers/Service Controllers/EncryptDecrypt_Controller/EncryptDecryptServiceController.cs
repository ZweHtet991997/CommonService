using CommonServices.Services.EncrypDecrypt_Service;
using Microsoft.AspNetCore.Mvc;

namespace CommonServices.Controllers.Service_Controllers.EncryptDecrypt_Controller
{
    public class EncryptDecryptServiceController : Controller
    {
        private readonly IAesService _aesService;

        public EncryptDecryptServiceController(IAesService aesService)
        {
            _aesService = aesService;
        }

        [HttpPost("service/encrypt")]
        public IActionResult Encryption(string plainText)
        {
            var dataResult = _aesService.EncryptString(plainText);
            return dataResult is not null ? StatusCode(StatusCodes.Status200OK, dataResult) :
                StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
