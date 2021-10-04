namespace ChatApp.StockBot.MessageBroker.Publishers
{
    public interface IPublisher<in T> where T : IntegrationEvent
    {
        void Publish(T @event);
    }
}