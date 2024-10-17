using NotificationCommonLibrary.Interfaces.Listeners;
using NotificationModelLibrary.Events;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitLibrary.Listeners
{
    public sealed class RabbitListener : IMessageListener
    {
        #region Поля

        private IConfiguration Configuration { get; }

        private string Queue { get; }

        private IConnection? Connection { get; set; }

        private IModel? Channel { get; set; }

        private AsyncEventingBasicConsumer? Consumer { get; set; }

        private Queue<IMessageHandler> Handlers { get; }

        private Queue<IAsyncMessageHandler> AsyncHandlers { get; }

        #endregion

        public RabbitListener(IConfiguration configuration, string queue)
        {
            Configuration = configuration;
            Queue = queue;

            Handlers = new Queue<IMessageHandler>();
            AsyncHandlers = new Queue<IAsyncMessageHandler>();
        }

        public void AddHandler(IMessageHandler messageHandler)
        {
            Handlers.Enqueue(messageHandler);
        }

        public void AddHandler(IAsyncMessageHandler messageHandler)
        {
            AsyncHandlers.Enqueue(messageHandler);
        }

        public void Open()
        {
            var endpointConf = Configuration.GetSection($"AppSettings:Listeners:RabbitMQ:Endpoints:{Queue}");

            var hostName = endpointConf["HostName"] ?? throw new NullReferenceException($"Для {Queue} не указан HostName");
            var virtualHost = endpointConf["VirtualHost"] ?? throw new NullReferenceException($"Для {Queue} не указан VirtualHost");
            var userName = endpointConf["UserName"] ?? throw new NullReferenceException($"Для {Queue} не указан UserName");
            var password = endpointConf["Password"] ?? throw new NullReferenceException($"Для {Queue} не указан Password");
            var queue = endpointConf["Queue"] ?? throw new NullReferenceException($"Для {Queue} не указан Queue");

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                VirtualHost = virtualHost,
                UserName = userName,
                Password = password,
                DispatchConsumersAsync = true
            };

            Connection = factory.CreateConnection();
            Channel = Connection.CreateModel();

            Channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Consumer = new AsyncEventingBasicConsumer(Channel);
            Consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var param = PropertiesToDictionary(ea.BasicProperties);

                var args = new MessageReceivedEventArgs()
                {
                    QueueName = queue,
                    Message = content,
                    Param = param,
                    Failed = false,
                    Handled = false,
                    Rejected = false,
                    Resended = false,
                };

                foreach (var handler in Handlers)
                {
                    handler.OnMessageRecieved(args);

                    if (args.Resended)
                    {
                        Channel.BasicReject(ea.DeliveryTag, true);
                        return;
                    }

                    if (args.Rejected)
                    {
                        Channel.BasicReject(ea.DeliveryTag, false);
                        return;
                    }

                    if (args.Handled)
                    {
                        Channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    }
                }

                foreach (var handler in AsyncHandlers)
                {
                    await handler.OnMessageRecievedAsync(args);

                    if (args.Resended)
                    {
                        Channel.BasicReject(ea.DeliveryTag, true);
                        return;
                    }

                    if (args.Rejected)
                    {
                        Channel.BasicReject(ea.DeliveryTag, false);
                        return;
                    }

                    if (args.Handled)
                    {
                        Channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    }
                }

                Channel.BasicAck(ea.DeliveryTag, false);
            };

            Channel.BasicConsume(queue, false, Consumer);
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            Channel?.Close();
            Connection?.Close();
        }

        private static Dictionary<string, object> PropertiesToDictionary(IBasicProperties properties)
        {
            var dict = new Dictionary<string, object>()
            {
                { "AppId", properties.AppId },
                { "ClusterId", properties.ClusterId },
                { "ContentEncoding", properties.ContentEncoding },
                { "ContentType", properties.ContentType },
                { "CorrelationId", properties.CorrelationId },
                { "DeliveryMode", properties.DeliveryMode },
                { "Expiration", properties.Expiration },
                { "MessageId", properties.MessageId },
                { "Persistent", properties.Persistent },
                { "Priority", properties.Priority },
                { "ReplyTo", properties.ReplyTo },
                { "Type", properties.Type },
                { "UserId", properties.UserId }
            };

            if (properties.Headers != null)
            {
                foreach (var h in properties.Headers.Where(x => x.Value != null))
                {
                    dict[h.Key] = h.Value;
                }
            }

            return dict;
        }
    }
}