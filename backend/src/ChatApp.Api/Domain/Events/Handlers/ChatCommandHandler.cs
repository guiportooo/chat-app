namespace ChatApp.Api.Domain.Events.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Events;
    using Exceptions;
    using IntegrationEvents;
    using IntegrationEvents.Publishers;
    using MediatR;
    using Services;

    public class ChatCommandHandler : INotificationHandler<MessageSent>
    {
        private readonly IChatCommandParser _commandParser;
        private readonly IStockQuoteRequestedPublisher _publisher;

        public ChatCommandHandler(IChatCommandParser commandParser, IStockQuoteRequestedPublisher publisher)
        {
            _commandParser = commandParser;
            _publisher = publisher;
        }

        public Task Handle(MessageSent @event, CancellationToken cancellationToken)
        {
            var text = @event.Text;
            
            if (!_commandParser.IsCommand(text))
                return Task.CompletedTask;

            var (command, value) = _commandParser.Parse(text);

            var integrationEvent = command switch
            {
                ChatCommands.Stock => new StockQuoteRequested(value, @event.RoomCode),
                _ => throw new InvalidChatCommandException(text)
            };
            
            _publisher.Publish(integrationEvent);
            return Task.CompletedTask;
        }
    }
}