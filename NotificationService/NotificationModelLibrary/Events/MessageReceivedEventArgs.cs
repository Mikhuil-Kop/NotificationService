namespace NotificationModelLibrary.Events
{
    public class MessageReceivedEventArgs
    {
        public required string QueueName { get; set; }
        public required string Message { get; set; }
        public required Dictionary<string, object> Param { get; set; }
        public bool Handled { get; set; }
        public bool Failed { get; set; }
        public bool Rejected { get; set; }
        public bool Resended { get; set; }
    }
}