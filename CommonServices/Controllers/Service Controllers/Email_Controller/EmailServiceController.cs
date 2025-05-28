using CommonServices.Models.Enums.Email;
using CommonServices.Resources;
using CommonServices.Services.Email_Service;
using Microsoft.AspNetCore.Mvc;

namespace CommonServices.Controllers.Service_Controllers.Email_Controller
{
    public class EmailServiceController : Controller
    {
        private readonly ISendMailService _mailService;
        private readonly IMailTypeMapper _mailTypeMapper;
        private readonly IVertificationService _vertificationService;

        public EmailServiceController(ISendMailService mailService,
            IVertificationService vertificationService,
            IMailTypeMapper mailTypeMapper)
        {
            _mailService = mailService;
            _vertificationService = vertificationService;
            _mailTypeMapper = mailTypeMapper;
        }

        [HttpPost("service/email")]
        public async Task<IActionResult> SendMail([FromBody] MailRequestModel model)
        {
            string emailContent = model.MailType is null
        ? model.Body
        : _mailTypeMapper.GetMailType(model);

            EmailMetadata emailMetadata = new(model.ToMail, model.CCMail, model.Subject, emailContent);
            await _mailService.Send(emailMetadata);

            return Ok();
        }

        [HttpPost("service/getotp")]
        public IActionResult GetOTP(string email)
        {
            var dataResult = _vertificationService.GenerateAndStoreCode(email);
            return Ok(dataResult);
        }

        [HttpPost("service/validateotp")]
        public IActionResult ValidateOTPCode([FromHeader]string email,[FromHeader] int code)
        {
            var validateOTPResult = _vertificationService.ValidateCode(email, code);

            switch (validateOTPResult)
            {
                case EnumOTPResponse.Valid:
                    return StatusCode(StatusCodes.Status200OK, ResponseMessageResource.Success);

                case EnumOTPResponse.Expired:
                    return StatusCode(StatusCodes.Status410Gone, ResponseMessageResource.OTPExpired);

                default:
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseMessageResource.InvalidOTP);
            }
        }

        [HttpPost("service/validateotp/successmail")]
        public IActionResult SendSuccessActivateMail([FromBody] MailRequestModel model)
        {
            model.MailType = EnumMailType.SuccessActivate.ToString();
            model.Subject = EmailSubjectResource.AccountActivated;

            var emailMetadata = new EmailMetadata(model.ToMail, model.CCMail, model.Subject,
                _mailTypeMapper.GetMailType(model));

            // send account activate success email
            _mailService.Send(emailMetadata);
            return Ok();
        }

    }
}
