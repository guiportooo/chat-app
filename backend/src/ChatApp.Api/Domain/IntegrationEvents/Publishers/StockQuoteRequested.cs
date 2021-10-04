namespace ChatApp.Api.Domain.IntegrationEvents.Publishers
{
    public class StockQuoteRequested : IntegrationEvent
    {
        public StockQuoteRequested(string stockCode, string roomCode)
        {
            StockCode = stockCode;
            RoomCode = roomCode;
        }

        public string StockCode { get; }
        public string RoomCode { get; }
    }
}