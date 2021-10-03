namespace ChatApp.Api.Domain.IntegrationEvents.Publishers
{
    public interface IPublisher<in T> where T : IntegrationEvent
    {
        void Publish(T @event);
    }
}