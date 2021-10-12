namespace ChatApp.Api.Hub
{
    using Microsoft.AspNetCore.SignalR;

    public class SignalRHub : Hub<IChatClient>
    {
    }
}