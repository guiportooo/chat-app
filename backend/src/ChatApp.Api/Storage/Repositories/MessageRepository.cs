namespace ChatApp.Api.Storage.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Models;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Message>> GetLastByRoom(int roomId, int numberOfMessages) =>
            await _dbContext
                .Messages
                .Include(x => x.Room)
                .Include(x => x.User)
                .Where(x => x.RoomId == roomId)
                .OrderByDescending(x => x.Timestamp)
                .Take(numberOfMessages)
                .ToListAsync();
    }
}