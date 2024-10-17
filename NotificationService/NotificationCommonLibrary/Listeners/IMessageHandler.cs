using NotificationModelLibrary.Events;

namespace NotificationCommonLibrary.Interfaces.Listeners
{
    public interface IMessageHandler
    {
        void OnMessageRecieved(MessageReceivedEventArgs args);
    }
}