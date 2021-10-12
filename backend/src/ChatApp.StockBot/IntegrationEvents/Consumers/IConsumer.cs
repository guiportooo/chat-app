namespace ChatApp.StockBot.IntegrationEvents.Consumers
{
    using System.Threading.Tasks;
    using MessageBroker;

    public interface IConsumer<in T> where T : IntegrationEvent
    {
        Task Consume(T @event);
    }
}