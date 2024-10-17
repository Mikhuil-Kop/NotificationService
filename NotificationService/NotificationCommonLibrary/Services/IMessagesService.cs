using NotificationModelLibrary.Requests;
using NotificationModelLibrary.Responses;

namespace NotificationCommonLibrary.Services
{
    /// <summary>
    /// Класс для отправки сообщений пользователям.
    /// Не путать с <seealso cref="Senders.IMessageSender"/> и <seealso cref="Listeners.IMessageListener"/>.
    /// </summary>
    public interface IMessagesService
    {
        public Task<SendMessageResponse> SendMessageRequestAsync(SendMessageRequest request);
    }

    public static class MessagesServiceExtensions
    {
        public static Task<SendMessageResponse> SendMessageRequestAsync(this IMessagesService service, string recipientCode, string title, string message)
        {
            var request = new SendMessageRequest()
            {
                RecipientPrimaryCode = recipientCode,
                Title = title,
                Message = message
            };

            return service.SendMessageRequestAsync(request);
        }

        public static Task<SendMessageResponse> SendMessageRequestAsync(this IMessagesService service, string recipientCode1, string recipientCode2, string title, string message)
        {
            var request = new SendMessageRequest()
            {
                RecipientPrimaryCode = recipientCode1,
                RecipientSecondaryCode = recipientCode2,
                Title = title,
                Message = message
            };

            return service.SendMessageRequestAsync(request);
        }

        public static Task<SendMessageResponse> SendMessageRequestAsync(this IMessagesService service, string recipientCode, string title, string message, params byte[][] attachments)
        {
            var request = new SendMessageRequest()
            {
                RecipientPrimaryCode = recipientCode,
                Title = title,
                Message = message,
                Attachments = [.. attachments]
            };

            return service.SendMessageRequestAsync(request);
        }

        public static Task<SendMessageResponse> SendMessageRequestAsync(this IMessagesService service, string recipientCode1, string recipientCode2, string title, string message, params byte[][] attachments)
        {
            var request = new SendMessageRequest()
            {
                RecipientPrimaryCode = recipientCode1,
                RecipientSecondaryCode = recipientCode2,
                Title = title,
                Message = message,
                Attachments = [.. attachments]
            };

            return service.SendMessageRequestAsync(request);
        }
    }
}
