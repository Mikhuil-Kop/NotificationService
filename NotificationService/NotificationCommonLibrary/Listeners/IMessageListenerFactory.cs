namespace NotificationCommonLibrary.Interfaces.Listeners
{
    public interface IMessageListenerFactory : IDisposable
    {
        IMessageListener CreateListener(string queue);
    }
}