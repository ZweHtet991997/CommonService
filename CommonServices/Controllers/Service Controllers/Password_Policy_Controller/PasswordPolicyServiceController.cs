using CommonServices.Services.EncrypDecrypt_Service;
using CommonServices.Services.Password_Policy_Service;
using Microsoft.AspNetCore.Mvc;

namespace CommonServices.Controllers.Service_Controllers.Password_Policy_Controller
{
    public class PasswordPolicyServiceController : Controller
    {
        private readonly IAesService _aesService;

        public PasswordPolicyServiceController(IAesService aesService)
        {
            _aesService = aesService;
        }

        [HttpPost("service/passwordpolicy")]
        public IActionResult CheckPasswordPolicy([FromHeader] string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                var decryptedPassword = _aesService.DecryptString(password);
                var dataResult = PasswordPolicyService.ValidatePassword(decryptedPassword);
                if (!string.IsNullOrEmpty(dataResult))
                {
                    return BadRequest(dataResult);
                }
                return Ok();
            }
            return BadRequest();

        }
    }
}
