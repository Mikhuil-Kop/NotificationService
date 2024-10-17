namespace NotificationCommonLibrary.Interfaces.Senders
{
    public interface IMessageSender
    {
        Task SendMessageAsync(string endpoint, string message, Dictionary<string, object>? param = null);
    }
}