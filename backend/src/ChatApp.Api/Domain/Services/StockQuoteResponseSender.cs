namespace ChatApp.Api.Domain.Services
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Commands;
    using IntegrationEvents.Consumers;
    using MediatR;
    using Models;

    public interface IStockQuoteResponseSender
    {
        Task Send(StockQuoteResponded? @event);
    }

    public class StockQuoteResponseSender : IStockQuoteResponseSender
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public StockQuoteResponseSender(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Send(StockQuoteResponded? @event)
        {
            if (@event is null)
                return;

            @event.UserName = StockBotUser.BotUserName;
            var command = _mapper.Map<SendMessage>(@event);
            await _mediator.Send(command);
        }
    }
}