namespace ChatApp.Api.WebSocket
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Events;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class MessageSentHandler : INotificationHandler<MessageSent>
    {
        private readonly IHubContext<ChatHub, IChatClient> _chatHub;
        private readonly ILogger<MessageSentHandler> _logger;

        public MessageSentHandler(IHubContext<ChatHub, IChatClient> chatHub, ILogger<MessageSentHandler> logger)
        {
            _chatHub = chatHub;
            _logger = logger;
        }

        public async Task Handle(MessageSent message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending message to hub: {Message}", message);
            await _chatHub.Clients.All.ReceiveMessage(message);
        }
    }
}