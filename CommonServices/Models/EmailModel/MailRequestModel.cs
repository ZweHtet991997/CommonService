namespace CommonServices.Models.EmailModel
{
    public class MailRequestModel
    {
        public string Name { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ToMail { get; set; } = null!;
        public string CCMail { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string MailType { get; set; } = null!;
    }
}
