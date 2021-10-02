namespace ChatApp.Api.Storage.Repositories
{
    using System.Threading.Tasks;
    using Domain.Models;
    using Domain.Repositories;

    public class MessageRepository : IMessageRepository
    {
        private readonly ChatAppDbContext _dbContext;

        public MessageRepository(ChatAppDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Message message)
        {
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Message?> Get(int id) => await _dbContext.Messages.FindAsync(id);
    }
}