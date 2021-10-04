namespace ChatApp.Api.Domain.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Events;
    using Exceptions;
    using MediatR;
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

        public SendMessageHandler(IUserRepository userRepository,
            IRoomRepository roomRepository,
            IMessageRepository messageRepository,
            IChatCommandParser commandParser,
            IMediator mediator)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _messageRepository = messageRepository;
            _commandParser = commandParser;
            _mediator = mediator;
        }

        public async Task<Message?> Handle(SendMessage command, CancellationToken cancellationToken)
        {
            var (roomCode, userName, text) = command;
            var room = await GetRoom(roomCode);
            var user = await GetUser(userName);
            var message = new Message(text, room, user);
            var @event = new MessageSent(message.Text, message.Timestamp, userName, roomCode);
            await _mediator.Publish(@event, cancellationToken);

            if (!message.ShouldBeSaved) return null;

            await _messageRepository.Add(message);
            return message;
        }

        private async Task<Room> GetRoom(string roomCode)
        {
            var room = await _roomRepository.GetByCode(roomCode);

            if (room is null)
                throw new RoomNotFoundException(roomCode);

            return room;
        }

        private async Task<User> GetUser(string userName)
        {
            if (userName == StockBotUser.BotUserName)
                return new StockBotUser();

            var user = await _userRepository.GetByUserName(userName);

            if (user is null)
                throw new UserNotFoundException(userName);

            return user;
        }
    }
}