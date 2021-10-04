namespace ChatApp.Api.MessageBroker.Consumers.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Commands;
    using Domain.IntegrationEvents.Consumers;
    using Domain.Models;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public interface IStockQuoteResponseSender
    {
        Task Send(StockQuoteResponded? @event);
    }

    public class StockQuoteResponseSender : IStockQuoteResponseSender
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StockQuoteResponseSender(IServiceScopeFactory serviceScopeFactory) =>
            _serviceScopeFactory = serviceScopeFactory;

        public async Task Send(StockQuoteResponded? @event)
        {
            if (@event is null)
                return;

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