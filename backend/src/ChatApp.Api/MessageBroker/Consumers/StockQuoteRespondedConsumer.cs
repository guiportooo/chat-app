namespace ChatApp.Api.MessageBroker.Consumers
{
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Commands;
    using Domain.IntegrationEvents.Consumers;
    using Domain.IntegrationEvents.Publishers;
    using Domain.Services;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class StockQuoteRespondedConsumer : BackgroundService
    {
        private readonly MessageBrokerSettings _settings;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<StockQuoteRespondedConsumer> _logger;

        public StockQuoteRespondedConsumer(IOptions<MessageBrokerSettings> settings,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<StockQuoteRespondedConsumer> logger)
        {
            _settings = settings.Value;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            var factory = new ConnectionFactory { HostName = _settings.Host, Port = _settings.Port };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _settings.BrokerName, type: ExchangeType.Direct);
            _queueName = _channel.QueueDeclare().QueueName;
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _channel.QueueBind(queue: _queueName,
                exchange: _settings.BrokerName,
                routingKey: nameof(StockQuoteResponded));

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (_, eventArgs) =>
            {
                var body = eventArgs.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                _logger.LogInformation("Stock quote response consumed: {Message}", message);
                var stockQuoteResponded = JsonSerializer.Deserialize<StockQuoteResponded>(message);
                using var scope = _serviceScopeFactory.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<IStockQuoteResponseSender>();
                sender.Send(stockQuoteResponded);
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