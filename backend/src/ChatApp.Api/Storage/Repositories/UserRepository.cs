namespace ChatApp.Api.Storage.Repositories
{
    using System.Threading.Tasks;
    using Domain.Models;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly ChatAppDbContext _dbContext;

        public UserRepository(ChatAppDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> Get(int id) => await _dbContext.Users.FindAsync(id);

        public async Task<User?> GetByUserName(string userName) =>
            await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);

        public async Task<User?> GetByUserNameAndPassword(string userName, string password) =>
            await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
    }
}