namespace ChatApp.Api.MessageBroker.Publishers
{
    using System.Text;
    using System.Text.Json;
    using Domain.IntegrationEvents;
    using Domain.IntegrationEvents.Publishers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;

    public class Publisher<T> : IPublisher<T> where T : IntegrationEvent
    {
        private readonly MessageBrokerSettings _settings;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<IPublisher<T>> _logger;

        public Publisher(IOptions<MessageBrokerSettings> settings, ILogger<Publisher<T>> logger)
        {
            _settings = settings.Value;
            _logger = logger;
            var factory = new ConnectionFactory { HostName = _settings.Host, Port = _settings.Port };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _settings.BrokerName, type: ExchangeType.Direct);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        public void Publish(T @event)
        {
            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: _settings.BrokerName,
                routingKey: @event.GetType().Name,
                basicProperties: null,
                body: body);
            _logger.LogInformation("Publishing {Type}: {Message}", typeof(T).Name, message);
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e) =>
            _logger.LogInformation("--> RabbitMQ Connection Shutdown");

        public void Dispose()
        {
            if (!_channel.IsOpen)
                return;

            _channel.Close();
            _connection.Close();
        }
    }
}