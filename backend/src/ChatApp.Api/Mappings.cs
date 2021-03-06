namespace ChatApp.Api
{
    using AutoMapper;
    using Requests = HttpIn.Requests;
    using Responses = HttpIn.Responses;
    using Commands = Domain.Commands;
    using Models = Domain.Models;
    using Events = Domain.Events;
    using Consumers = Domain.IntegrationEvents.Consumers;

    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Requests.RegisterUser, Commands.RegisterUser>();
            CreateMap<Commands.RegisterUser, Models.User>();
            CreateMap<Models.User, Responses.User>();
            CreateMap<Requests.AuthenticateUser, Commands.AuthenticateUser>();
            CreateMap<Models.Message, Responses.MessageSent>();
            CreateMap<Models.Message, Responses.Message>();
            CreateMap<Models.Message, Events.MessageSent>();
            CreateMap<Consumers.StockQuoteResponded, Commands.SendMessage>();
        }
    }
}