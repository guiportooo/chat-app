namespace ChatApp.Api.MessageBroker.Consumers
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Commands;
    using Domain.IntegrationEvents.Consumers;
    using Domain.Models;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class StockQuoteRespondedConsumer : Consumer<StockQuoteResponded>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StockQuoteRespondedConsumer(IOptions<MessageBrokerSettings> settings,
            ILogger<StockQuoteRespondedConsumer> logger,
            IServiceScopeFactory serviceScopeFactory)
            : base(settings, logger) =>
            _serviceScopeFactory = serviceScopeFactory;

        public override async Task Consume(StockQuoteResponded @event)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            if (mapper is null)
                throw new ArgumentNullException(nameof(mapper), "Could not instantiate Mapper");

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            if (mediator is null)
                throw new ArgumentNullException(nameof(mediator), "Could not instantiate Mediator");

            @event.UserName = StockBotUser.BotUserName;
            var command = mapper.Map<SendMessage>(@event);
            await mediator.Send(command);
        }
    }
}