namespace ChatApp.Api.WebSocket
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub<IChatClient>
    {
    }
}