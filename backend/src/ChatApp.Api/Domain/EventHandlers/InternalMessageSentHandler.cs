namespace ChatApp.Api.Domain.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class InternalMessageSentHandler : INotificationHandler<MessageSent>
    {
        private readonly ILogger<InternalMessageSentHandler> _logger;

        public InternalMessageSentHandler(ILogger<InternalMessageSentHandler> logger) => _logger = logger;

        public Task Handle(MessageSent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message '{Text}' sent from {UserName} to room {RoomCode}",
                @event.Text, @event.UserName, @event.RoomCode);
            return Task.CompletedTask;
        }
    }
}