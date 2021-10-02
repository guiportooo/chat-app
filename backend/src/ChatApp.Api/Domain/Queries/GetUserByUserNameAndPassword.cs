namespace ChatApp.Api.Domain.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Repositories;

    public record GetUserByUserNameAndPassword(string UserName, string Password) : IRequest<User?>;

    public class GetUserByUserNameAndPasswordHandler : IRequestHandler<GetUserByUserNameAndPassword, User?>
    {
        private readonly IUserRepository _repository;

        public GetUserByUserNameAndPasswordHandler(IUserRepository repository) => _repository = repository;

        public Task<User?> Handle(GetUserByUserNameAndPassword query, CancellationToken cancellationToken)
        {
            var (userName, password) = query;
            return _repository.GetByUserNameAndPassword(userName, password);
        }
    }
}