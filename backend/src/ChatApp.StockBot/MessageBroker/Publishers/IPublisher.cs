namespace ChatApp.StockBot.MessageBroker.Publishers
{
    using IntegrationEvents;

    public interface IPublisher<in T> where T : IntegrationEvent
    {
        void Publish(T @event);
    }
}