using NotificationCommonLibrary.Interfaces.Listeners;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RabbitLibrary.Listeners
{
    public sealed class RabbitListenerFactory : IMessageListenerFactory
    {
        private ILogger Logger { get; }

        private ILoggerFactory LoggerFactory { get; }

        private IConfiguration Configuration { get; }

        private LinkedList<RabbitListener> Listeners { get; }


        public RabbitListenerFactory(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            Logger = loggerFactory.CreateLogger<RabbitListenerFactory>();
            LoggerFactory = loggerFactory;
            Configuration = configuration;
            Listeners = new LinkedList<RabbitListener>();
        }

        public IMessageListener CreateListener(string queue)
        {
            var listener = new RabbitListener(Configuration, queue);

            Listeners.AddLast(listener);

            return listener;
        }

        public void Dispose()
        {
            foreach (var listener in Listeners)
            {
                listener.Dispose();
            }
        }
    }
}