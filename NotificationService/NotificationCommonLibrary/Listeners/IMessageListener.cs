namespace NotificationCommonLibrary.Listeners
{
    /// <summary>
    /// Класс для получения сообщений из конкретной очереди через Message Broker.
    /// </summary>
    public interface IMessageListener : IDisposable
    {
        void AddHandler(IMessageHandler messageHandler);
        void AddHandler(IAsyncMessageHandler messageHandler);
        void Open();
        void Close();
    }
}