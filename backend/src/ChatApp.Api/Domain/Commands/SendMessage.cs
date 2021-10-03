namespace ChatApp.Api.Domain.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Events;
    using Exceptions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Models;
    using Repositories;
    using Services;

    public record SendMessage(string RoomCode, string UserName, string Text) : IRequest<Message?>;

    public class SendMessageHandler : IRequestHandler<SendMessage, Message?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IChatCommandParser _commandParser;
        private readonly IMediator _mediator;
        private readonly ILogger<SendMessageHandler> _logger;

        public SendMessageHandler(IUserRepository userRepository,
            IRoomRepository roomRepository,
            IMessageRepository messageRepository,
            IChatCommandParser commandParser,
            IMediator mediator,
            ILogger<SendMessageHandler> logger)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _messageRepository = messageRepository;
            _commandParser = commandParser;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Message?> Handle(SendMessage command, CancellationToken cancellationToken)
        {
            var (roomCode, userName, text) = command;
            var room = await _roomRepository.GetByCode(roomCode);

            if (room is null)
                throw new RoomNotFoundException(roomCode);

            var user = await _userRepository.GetByUserName(userName);

            if (user is null)
                throw new UserNotFoundException(userName);

            var message = new Message(text, room.Id, user.Id);

            var @event = new MessageSent(message.Text, message.Timestamp, userName, roomCode);
            await _mediator.Publish(@event, cancellationToken);
            _logger.LogInformation("Message '{Text}' sent from {UserName} to room {RoomCode}",
                text, userName, roomCode);
            
            if (_commandParser.IsCommand(text))
                return null;
            
            await _messageRepository.Add(message);
            return message;
        }
    }
}