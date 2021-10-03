namespace ChatApp.StockBot.MessageBroker
{
    public class MessageBrokerSettings
    {
        public const string Name = "MessageBroker";

        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string BrokerName { get; set; } = null!;
    }
}