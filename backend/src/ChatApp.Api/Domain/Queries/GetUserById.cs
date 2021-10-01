namespace ChatApp.Api.Domain.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Repositories;

    public record GetUserById(int Id) : IRequest<User?>;

    public class GetUserByIdHandler : IRequestHandler<GetUserById, User?>
    {
        private readonly IUserRepository _repository;

        public GetUserByIdHandler(IUserRepository repository) => _repository = repository;

        public Task<User?> Handle(GetUserById query, CancellationToken cancellationToken) => _repository.Get(query.Id);
    }
}