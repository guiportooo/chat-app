namespace ChatApp.Api.Domain.Repositories
{
    using System.Threading.Tasks;
    using Models;

    public interface IUserRepository
    {
        Task Add(User user);
        Task<User?> Get(int id);
        Task<User?> GetByUserName(string userName);
        Task<User?> GetByUserNameAndPassword(string userName, string password);
    }
}