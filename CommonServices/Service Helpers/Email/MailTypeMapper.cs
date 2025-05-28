using CommonServices.Models.Enums.Email;
using CommonServices.Resources;

namespace CommonServices.Service_Helpers.Email
{
    public class MailTypeMapper : IMailTypeMapper
    {
        private readonly IVertificationService _vertificationService;

        public MailTypeMapper(IVertificationService vertificationService)
        {
            _vertificationService = vertificationService;
        }

        public string GetMailType(MailRequestModel model)
        {
            if (Enum.TryParse<EnumMailType>(model.MailType, out var mailType))
            {
                switch (mailType)
                {
                    case EnumMailType.Activate:
                        return EmailTemplateResource
                            .AccountActivateTemplate(model.ProjectName,
                            _vertificationService.GenerateAndStoreCode(model.ToMail));

                    case EnumMailType.SuccessActivate:
                        string[] userName = model.ToMail.Split('@');
                        return EmailTemplateResource.SuccessActivateTemplate(model.ProjectName, userName[0]);

                    case EnumMailType.ContactUs:
                        return EmailTemplateResource.ContactUsTemplate(model.Name,
                            model.PhoneNo, model.Email, model.Body);

                    default:
                        return "Invalid mail type.";
                }
            }
            else
            {
                // Handle invalid parse
                return string.Empty;
            }

        }
    }
}
