namespace NotificationCommonLibrary.Listeners
{
    /// <summary>
    /// Класс для получения сообщений через Message Broker.
    /// Создает <see cref="IMessageListener"/> для прослушивания конкретной очереди.
    /// </summary>
    public interface IMessageListenerFactory : IDisposable
    {
        IMessageListener CreateListener(string queue);
    }
}