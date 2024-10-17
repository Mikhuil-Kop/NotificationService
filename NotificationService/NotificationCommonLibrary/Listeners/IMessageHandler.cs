using NotificationModelLibrary.Events;

namespace NotificationCommonLibrary.Listeners
{
    public interface IMessageHandler
    {
        void OnMessageRecieved(MessageReceivedEventArgs args);
    }
}