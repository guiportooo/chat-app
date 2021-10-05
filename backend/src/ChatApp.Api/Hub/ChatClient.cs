namespace ChatApp.Api.Hub
{
    using System.Threading.Tasks;
    using Domain.Events;

    public interface IChatClient
    {
        Task ReceiveMessage(MessageSent message);
    }
}