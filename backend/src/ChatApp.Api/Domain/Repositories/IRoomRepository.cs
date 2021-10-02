namespace ChatApp.Api.Domain.Repositories
{
    using System.Threading.Tasks;
    using Models;

    public interface IRoomRepository
    {
        Task<Room?> GetByCode(string code);
    }
}