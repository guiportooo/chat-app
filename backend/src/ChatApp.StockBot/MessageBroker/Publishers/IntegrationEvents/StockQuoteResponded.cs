namespace ChatApp.StockBot.MessageBroker.Publishers.IntegrationEvents
{
    public class StockQuoteResponded : IntegrationEvent
    {
        public StockQuoteResponded(string stockCode, string stockValue, string roomCode)
        {
            Text = $"{stockCode} quote is ${stockValue} per share.";
            RoomCode = roomCode;
        }

        public string Text { get; set; }
        public string RoomCode { get; set; }
    }
}