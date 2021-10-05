namespace ChatApp.Api.Hub
{
    using System.Threading.Tasks;
    using Domain.Events;
    using Domain.IntegrationEvents.Hub;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class ChatHub : IHub
    {
        private readonly IHubContext<SignalRHub, IChatClient> _chatHub;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IHubContext<SignalRHub, IChatClient> chatHub, ILogger<ChatHub> logger)
        {
            _chatHub = chatHub;
            _logger = logger;
        }

        public async Task Send(MessageSent message)
        {
            _logger.LogInformation("Sending message to hub: {Message}", message);
            await _chatHub.Clients.All.ReceiveMessage(message);
        }
    }
}