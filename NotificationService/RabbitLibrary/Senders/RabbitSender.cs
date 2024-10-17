using NotificationCommonLibrary.Interfaces.Senders;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;

namespace RabbitLibrary.Senders
{
    public class RabbitSender : IMessageSender
    {
        #region Поля

        private IConfiguration Configuration { get; }

        #endregion

        public RabbitSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Task SendMessageAsync(string endpoint, string message, Dictionary<string, object>? param = null)
        {
            var endpointConf = Configuration.GetSection($"AppSettings:Senders:RabbitMQ:Endpoints:{endpoint}");

            var hostName = endpointConf["HostName"] ?? throw new NullReferenceException($"Для {endpoint} не указан HostName");
            var virtualHost = endpointConf["VirtualHost"] ?? throw new NullReferenceException($"Для {endpoint} не указан VirtualHost");
            var userName = endpointConf["UserName"] ?? throw new NullReferenceException($"Для {endpoint} не указан UserName");
            var password = endpointConf["Password"] ?? throw new NullReferenceException($"Для {endpoint} не указан Password");
            var queue = endpointConf["Queue"];
            var exchange = endpointConf["Exchange"];

            if (queue == null && exchange == null)
            {
                throw new NullReferenceException($"Для {endpoint} не указан Queue или Exchange");
            }

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                VirtualHost = virtualHost,
                UserName = userName,
                Password = password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var body = Encoding.UTF8.GetBytes(message);

            var properties = DictionaryToProperties(channel, param ?? new Dictionary<string, object>());

            channel.BasicPublish(exchange: exchange ?? "", routingKey: queue ?? "", basicProperties: properties, body: body);

            return Task.CompletedTask;
        }

        private static IBasicProperties DictionaryToProperties(IModel channel, Dictionary<string, object> param)
        {
            var properties = channel.CreateBasicProperties();

            var defaultProperties = new string[] { "AppId", "ClusterId", "ContentEncoding", "ContentType", "CorrelationId", "DeliveryMode", "Expiration", "MessageId", "Persistent", "Priority", "ReplyTo", "Type", "UserId" };

            properties.AppId = param.GetValueOrDefault("AppId")?.ToString();
            properties.ClusterId = param.GetValueOrDefault("ClusterId")?.ToString();
            properties.ContentEncoding = param.GetValueOrDefault("ContentEncoding")?.ToString();
            properties.ContentType = param.GetValueOrDefault("ContentType")?.ToString();
            properties.CorrelationId = param.GetValueOrDefault("CorrelationId")?.ToString();
            properties.DeliveryMode = byte.Parse(param.GetValueOrDefault("DeliveryMode")?.ToString() ?? "0");
            properties.Expiration = param.GetValueOrDefault("Expiration")?.ToString();
            properties.MessageId = param.GetValueOrDefault("MessageId")?.ToString();
            properties.Persistent = bool.Parse(param.GetValueOrDefault("Persistent")?.ToString() ?? "false");
            properties.Priority = byte.Parse(param.GetValueOrDefault("Priority")?.ToString() ?? "0");
            properties.ReplyTo = param.GetValueOrDefault("ReplyTo")?.ToString();
            properties.Type = param.GetValueOrDefault("Type")?.ToString();
            properties.UserId = param.GetValueOrDefault("UserId")?.ToString();

            properties.Headers = param.
                Where(x => !defaultProperties.Contains(x.Key)).
                ToDictionary(x => x.Key, x => x.Value);

            return properties;
        }
    }
}