using NotificationModelLibrary.Events;

namespace NotificationCommonLibrary.Listeners
{
    public interface IAsyncMessageHandler
    {
        Task OnMessageRecievedAsync(MessageReceivedEventArgs args);
    }
}