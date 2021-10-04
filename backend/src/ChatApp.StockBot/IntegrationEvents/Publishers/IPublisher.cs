namespace ChatApp.StockBot.IntegrationEvents.Publishers
{
    using MessageBroker;

    public interface IPublisher<in T> where T : IntegrationEvent
    {
        void Publish(T @event);
    }
}