namespace ChatApp.Api.Domain.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Exceptions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Models;
    using Repositories;

    public record SendMessage(string RoomCode, string UserName, string Text) : IRequest<Message>;

    public class SendMessageHandler : IRequestHandler<SendMessage, Message>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<SendMessageHandler> _logger;

        public SendMessageHandler(IUserRepository userRepository,
            IRoomRepository roomRepository,
            IMessageRepository messageRepository,
            ILogger<SendMessageHandler> logger)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task<Message> Handle(SendMessage request, CancellationToken cancellationToken)
        {
            var (roomCode, userName, text) = request;
            var room = await _roomRepository.GetByCode(roomCode);

            if (room is null)
                throw new RoomNotFoundException(roomCode);

            var user = await _userRepository.GetByUserName(userName);

            if (user is null)
                throw new UserNotFoundException(userName);

            var message = new Message(text, room.Id, user.Id);
            await _messageRepository.Add(message);
            _logger.LogInformation("Message '{Text}' sent from {UserName} to room {RoomCode}",
                text, userName, roomCode);
            return message;
        }
    }
}