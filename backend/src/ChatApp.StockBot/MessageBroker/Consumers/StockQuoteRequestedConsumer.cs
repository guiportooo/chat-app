namespace ChatApp.StockBot.MessageBroker.Consumers
{
    using System;
    using System.Threading.Tasks;
    using IntegrationEvents.Consumers;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Services;

    public class StockQuoteRequestedConsumer : Consumer<StockQuoteRequested>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StockQuoteRequestedConsumer(IOptions<MessageBrokerSettings> settings,
            ILogger<StockQuoteRequestedConsumer> logger,
            IServiceScopeFactory serviceScopeFactory) : base(settings, logger) =>
            _serviceScopeFactory = serviceScopeFactory;

        public override async Task Consume(StockQuoteRequested @event)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStockQuoteService>();

            if (service is null)
                throw new ArgumentNullException(nameof(service), "Could not instantiate Mapper");

            await service.SendStockQuote(@event);
        }
    }
}