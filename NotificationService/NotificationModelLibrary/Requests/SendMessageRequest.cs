namespace NotificationModelLibrary.Requests
{
    public class SendMessageRequest
    {
        public required string RecipientPrimaryCode { get; set; }
        public string? RecipientSecondaryCode { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public List<byte[]>? Attachments { get; set; }
    }
}
