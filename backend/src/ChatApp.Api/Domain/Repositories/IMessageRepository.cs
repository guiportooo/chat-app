namespace ChatApp.Api.Domain.Repositories
{
    using System.Threading.Tasks;
    using Models;

    public interface IMessageRepository
    {
        Task Add(Message message);
        Task<Message?> Get(int id);
    }
}