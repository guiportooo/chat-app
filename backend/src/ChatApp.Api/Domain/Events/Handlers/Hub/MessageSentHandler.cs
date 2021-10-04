namespace ChatApp.Api.Domain.Events.Handlers.Hub
{
    using System.Threading;
    using System.Threading.Tasks;
    using Events;
    using IntegrationEvents.Hub;
    using MediatR;

    public class MessageSentHandler : INotificationHandler<MessageSent>
    {
        private readonly IHub _hub;

        public MessageSentHandler(IHub hub) => _hub = hub;

        public async Task Handle(MessageSent @event, CancellationToken cancellationToken) => await _hub.Send(@event);
    }
}