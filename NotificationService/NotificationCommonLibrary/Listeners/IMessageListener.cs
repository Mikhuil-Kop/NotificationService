namespace NotificationCommonLibrary.Interfaces.Listeners
{
    public interface IMessageListener : IDisposable
    {
        void AddHandler(IMessageHandler messageHandler);
        void AddHandler(IAsyncMessageHandler messageHandler);
        void Open();
        void Close();
    }
}