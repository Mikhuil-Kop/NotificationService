using NotificationModelLibrary.Events;

namespace NotificationCommonLibrary.Interfaces.Listeners
{
    public interface IAsyncMessageHandler
    {
        Task OnMessageRecievedAsync(MessageReceivedEventArgs args);
    }
}