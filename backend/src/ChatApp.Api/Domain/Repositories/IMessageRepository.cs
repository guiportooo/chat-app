namespace ChatApp.Api.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IMessageRepository
    {
        Task Add(Message message);
        Task<Message?> Get(int id);
        Task<IEnumerable<Message>> GetLastByRoom(int roomId, int numberOfMessages);
    }
}