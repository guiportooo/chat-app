namespace ChatApp.Api.Storage.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Models;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class RoomRepository : IRoomRepository
    {
        private readonly ChatAppDbContext _dbContext;

        public RoomRepository(ChatAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<Room?> GetByCode(string code) => await _dbContext
            .Rooms
            .Where(x => x.Code == code)
            .FirstOrDefaultAsync();
    }
}