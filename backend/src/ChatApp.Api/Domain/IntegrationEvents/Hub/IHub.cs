namespace ChatApp.Api.Domain.IntegrationEvents.Hub
{
    using System.Threading.Tasks;
    using Events;

    public interface IHub
    {
        Task Send(MessageSent message);
    }
}