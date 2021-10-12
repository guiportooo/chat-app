namespace ChatApp.Api.Domain.IntegrationEvents.Consumers
{
    using System.Threading.Tasks;

    public interface IConsumer<in T> where T : IntegrationEvent
    {
        Task Consume(T @event);
    }
}