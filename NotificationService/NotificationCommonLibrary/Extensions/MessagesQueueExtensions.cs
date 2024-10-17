using NotificationCommonLibrary.Interfaces.Listeners;
using NotificationCommonLibrary.Interfaces.Senders;
using NotificationModelLibrary.Events;
using System.Text.Json;
using System.Xml.Linq;

namespace NotificationCommonLibrary.Extensions
{
    public static class MessagesQueueExtensions
    {
        public delegate void OnMessageRecievedSimpleHandler(string queueName, string message, Dictionary<string, object> param);
        public delegate void OnMessageRecievedQueueHandler(string message, Dictionary<string, object> param);
        public delegate void OnMessageRecievedArgsHandler(MessageReceivedEventArgs args);

        public delegate Task AsyncOnMessageRecievedSimpleHandler(string queueName, string message, Dictionary<string, object> param);
        public delegate Task AsyncOnMessageRecievedQueueHandler(string message, Dictionary<string, object> param);
        public delegate Task AsyncOnMessageRecievedArgsHandler(MessageReceivedEventArgs args);

        public static Task SendMessageAsync(this IMessageSender sender, string endpoint, XElement message)
        {
            var param = new Dictionary<string, object>()
            {
                { "ContentType", "xml" }
            };

            var messageText = message.ToString();

            return sender.SendMessageAsync(endpoint, messageText, param);
        }

        public static Task SendMessageAsync<T>(this IMessageSender sender, string endpoint, T message)
        {
            var type = typeof(T);
            var contentType = GetTypeShortName(type);

            var param = new Dictionary<string, object>()
            {
                { "ContentType", contentType }
            };

            var messageText = JsonSerializer.Serialize(message);

            return sender.SendMessageAsync(endpoint, messageText, param);
        }

        public static void AddHandler(this IMessageListener listener, OnMessageRecievedSimpleHandler onMessage)
        {
            listener.AddHandler(new StringMessageHandler()
            {
                OnMessage = onMessage
            });
        }

        public static void AddHandler(this IMessageListener listener, OnMessageRecievedQueueHandler onMessage)
        {
            listener.AddHandler(new QueueMessageHandler()
            {
                OnMessage = onMessage
            });
        }

        public static void AddHandler(this IMessageListener listener, OnMessageRecievedArgsHandler onMessage)
        {
            listener.AddHandler(new ArgsMessageHandler()
            {
                OnMessage = onMessage
            });
        }

        public static void AddHandler(this IMessageListener listener, AsyncOnMessageRecievedSimpleHandler onMessage)
        {
            listener.AddHandler(new AsyncStringMessageHandler()
            {
                OnMessage = onMessage
            });
        }

        public static void AddHandler(this IMessageListener listener, AsyncOnMessageRecievedQueueHandler onMessage)
        {
            listener.AddHandler(new AsyncQueueMessageHandler()
            {
                OnMessage = onMessage
            });
        }

        public static void AddHandler(this IMessageListener listener, AsyncOnMessageRecievedArgsHandler onMessage)
        {
            listener.AddHandler(new AsyncArgsMessageHandler()
            {
                OnMessage = onMessage
            });
        }

        private static string GetTypeShortName(Type type)
        {
            if (type.IsGenericType)
            {
                var typeName = type.Name.Split("`")[0];
                var genericTypes = type.GenericTypeArguments.Select(GetTypeShortName);

                return typeName + "<" + string.Join(", ", genericTypes) + ">";
            }
            else
            {
                return type.Name;
            }
        }

        #region

        private class StringMessageHandler : IMessageHandler
        {
            public required OnMessageRecievedSimpleHandler OnMessage { get; set; }

            public void OnMessageRecieved(MessageReceivedEventArgs args)
            {
                OnMessage.Invoke(args.QueueName, args.Message, args.Param);
            }
        }

        private class QueueMessageHandler : IMessageHandler
        {
            public required OnMessageRecievedQueueHandler OnMessage { get; set; }

            public void OnMessageRecieved(MessageReceivedEventArgs args)
            {
                OnMessage.Invoke(args.Message, args.Param);
            }
        }

        private class ArgsMessageHandler : IMessageHandler
        {
            public required OnMessageRecievedArgsHandler OnMessage { get; set; }

            public void OnMessageRecieved(MessageReceivedEventArgs args)
            {
                OnMessage.Invoke(args);
            }
        }

        private class AsyncStringMessageHandler : IAsyncMessageHandler
        {
            public required AsyncOnMessageRecievedSimpleHandler OnMessage { get; set; }

            public Task OnMessageRecievedAsync(MessageReceivedEventArgs args)
            {
               return OnMessage.Invoke(args.QueueName, args.Message, args.Param);
            }
        }

        private class AsyncQueueMessageHandler : IAsyncMessageHandler
        {
            public required AsyncOnMessageRecievedQueueHandler OnMessage { get; set; }

            public Task OnMessageRecievedAsync(MessageReceivedEventArgs args)
            {
               return OnMessage.Invoke(args.Message, args.Param);
            }
        }

        private class AsyncArgsMessageHandler : IAsyncMessageHandler
        {
            public required AsyncOnMessageRecievedArgsHandler OnMessage { get; set; }

            public Task OnMessageRecievedAsync(MessageReceivedEventArgs args)
            {
                return OnMessage.Invoke(args);
            }
        }

        #endregion
    }
}