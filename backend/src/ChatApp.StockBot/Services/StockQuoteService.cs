namespace ChatApp.StockBot.Services
{
    using System.Threading.Tasks;
    using HttpOut;
    using IntegrationEvents.Consumers;
    using IntegrationEvents.Publishers;

    public interface IStockQuoteService
    {
        Task SendStockQuote(StockQuoteRequested? stockQuoteRequested);
    }

    public class StockQuoteService : IStockQuoteService
    {
        private readonly IStooqHttpClient _httpClient;
        private readonly IStockQuoteCsvParser _parser;
        private readonly IStockQuoteRespondedPublisher _publisher;

        public StockQuoteService(IStooqHttpClient httpClient,
            IStockQuoteCsvParser parser,
            IStockQuoteRespondedPublisher publisher)
        {
            _httpClient = httpClient;
            _parser = parser;
            _publisher = publisher;
        }

        public Task SendStockQuote(StockQuoteRequested? stockQuoteRequested)
        {
            if (stockQuoteRequested is null)
                return Task.CompletedTask;

            var csv = _httpClient.GetStockQuote(stockQuoteRequested.StockCode).Result;
            var stockQuote = _parser.Parse(csv);

            if (stockQuote is null)
                return Task.CompletedTask;

            var stockQuoteResponded = new StockQuoteResponded(stockQuoteRequested.StockCode,
                stockQuote.Close,
                stockQuoteRequested.RoomCode);
            _publisher.Publish(stockQuoteResponded);
            return Task.CompletedTask;
        }
    }
}