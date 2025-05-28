namespace CommonServices.Models.EmailModel
{
    public class EmailMetadata
    {
        public string? ToAddress { get; set; }
        public string? CCAddress { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }

        public EmailMetadata(string? toAddress, string? ccAddress, string? subject, string? body)
        {
            ToAddress = toAddress;
            CCAddress = ccAddress;
            Subject = subject;
            Body = body;
        }
    }
}
