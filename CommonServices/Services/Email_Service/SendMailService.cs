namespace CommonServices.Services.Email_Service
{
    public class SendMailService : ISendMailService
    {
        private readonly IFluentEmail _fluentEmail;

        public SendMailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail
                ?? throw new ArgumentNullException(nameof(_fluentEmail));
        }

        public async Task Send(EmailMetadata emailMetadata)
        {
            var email = _fluentEmail.To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .Body(emailMetadata.Body, isHtml: true);
            if (!string.IsNullOrEmpty(emailMetadata.CCAddress))
            {
                email.CC(emailMetadata.CCAddress);
            }
            await email.SendAsync();
        }
    }
}
