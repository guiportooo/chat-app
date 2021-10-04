namespace ChatApp.Api.WebSocket
{
    using System.Threading.Tasks;
    using Domain.Events;
    using Domain.IntegrationEvents.Hub;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class Hub : IHub
    {
        private readonly IHubContext<ChatHub, IChatClient> _chatHub;
        private readonly ILogger<Hub> _logger;

        public Hub(IHubContext<ChatHub, IChatClient> chatHub, ILogger<Hub> logger)
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