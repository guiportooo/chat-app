namespace ChatApp.StockBot.MessageBroker.Publishers
{
    using IntegrationEvents.Publishers;
    using MessageBroker;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class StockQuoteRespondedPublisher : Publisher<StockQuoteResponded>, IStockQuoteRespondedPublisher
    {
        public StockQuoteRespondedPublisher(IOptions<MessageBrokerSettings> settings,
            ILogger<StockQuoteRespondedPublisher> logger) : base(settings, logger)
        {
        }
    }
}