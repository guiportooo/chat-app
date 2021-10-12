namespace ChatApp.Api.MessageBroker.Publishers
{
    using Domain.IntegrationEvents.Publishers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class StockQuoteRequestedPublisher : Publisher<StockQuoteRequested>, IStockQuoteRequestedPublisher
    {
        public StockQuoteRequestedPublisher(IOptions<MessageBrokerSettings> settings,
            ILogger<StockQuoteRequestedPublisher> logger) : base(settings, logger)
        {
        }
    }
}