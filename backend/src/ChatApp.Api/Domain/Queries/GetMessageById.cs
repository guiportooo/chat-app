namespace ChatApp.Api.Domain.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Repositories;

    public record GetMessageById(int Id) : IRequest<Message?>;

    public class GetMessageByIdHandler : IRequestHandler<GetMessageById, Message?>
    {
        private readonly IMessageRepository _repository;

        public GetMessageByIdHandler(IMessageRepository repository) => _repository = repository;

        public Task<Message?> Handle(GetMessageById query, CancellationToken cancellationToken) =>
            _repository.Get(query.Id);
    }
}