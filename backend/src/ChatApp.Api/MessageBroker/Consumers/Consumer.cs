namespace ChatApp.Api.MessageBroker.Consumers
{
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.IntegrationEvents;
    using Domain.IntegrationEvents.Consumers;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public abstract class Consumer<T> : BackgroundService, IConsumer<T> where T : IntegrationEvent
    {
        private readonly MessageBrokerSettings _settings;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly ILogger<IConsumer<T>> _logger;

        protected Consumer(IOptions<MessageBrokerSettings> settings, ILogger<Consumer<T>> logger)
        {
            _settings = settings.Value;
            _logger = logger;
            var factory = new ConnectionFactory { HostName = _settings.Host, Port = _settings.Port };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _settings.BrokerName, type: ExchangeType.Direct);
            _queueName = _channel.QueueDeclare().QueueName;
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        public abstract Task Consume(T @event);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _channel.QueueBind(queue: _queueName,
                exchange: _settings.BrokerName,
                routingKey: typeof(T).Name);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (_, eventArgs) =>
            {
                var body = eventArgs.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                _logger.LogInformation("{Type} consumed: {Message}", typeof(T).Name, message);
                var @event = JsonSerializer.Deserialize<T>(message);

                if (@event is null) return;

                Consume(@event);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e) =>
            _logger.LogInformation("--> RabbitMQ Connection Shutdown");

        public override void Dispose()
        {
            if (!_channel.IsOpen)
                return;

            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}