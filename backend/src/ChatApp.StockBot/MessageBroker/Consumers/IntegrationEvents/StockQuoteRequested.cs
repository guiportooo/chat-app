namespace ChatApp.StockBot.MessageBroker.Consumers.IntegrationEvents
{
    public class StockQuoteRequested
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