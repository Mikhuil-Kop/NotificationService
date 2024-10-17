namespace NotificationCommonLibrary.Senders
{
    /// <summary>
    /// Класс для отправки сообщений через Message Broker.
    /// </summary>
    public interface IMessageSender
    {
        Task SendMessageAsync(string endpoint, string message, Dictionary<string, object>? param = null);
    }
}