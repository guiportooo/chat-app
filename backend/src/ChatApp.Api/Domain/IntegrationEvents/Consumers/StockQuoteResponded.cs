namespace ChatApp.Api.Domain.IntegrationEvents.Consumers
{
    public class StockQuoteResponded : IntegrationEvent
    {
        public StockQuoteResponded(string text, string roomCode, string userName)
        {
            Text = text;
            RoomCode = roomCode;
            UserName = userName;
        }

        public string Text { get; set; }
        public string RoomCode { get; set; }
        public string UserName { get; set; }
    }
}