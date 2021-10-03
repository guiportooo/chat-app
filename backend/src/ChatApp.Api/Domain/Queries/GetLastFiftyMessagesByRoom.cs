namespace ChatApp.Api.Domain.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Exceptions;
    using MediatR;
    using Models;
    using Repositories;

    public record GetLastFiftyMessagesByRoom(string RoomCode) : IRequest<IReadOnlyCollection<Message>>;

    public class GetLastFiftyMessagesByRoomHandler : IRequestHandler<GetLastFiftyMessagesByRoom, IReadOnlyCollection<Message>>
    {
        private const int numberOfMessages = 50;
        private readonly IRoomRepository _roomRepository;
        private readonly IMessageRepository _messageRepository;

        public GetLastFiftyMessagesByRoomHandler(IRoomRepository roomRepository, IMessageRepository messageRepository)
        {
            _roomRepository = roomRepository;
            _messageRepository = messageRepository;
        }

        public async Task<IReadOnlyCollection<Message>> Handle(GetLastFiftyMessagesByRoom query, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetByCode(query.RoomCode);

            if (room is null)
                throw new RoomNotFoundException(query.RoomCode);

            return (await _messageRepository.GetLastByRoom(room.Id, numberOfMessages)).ToList();
        }
    }
}