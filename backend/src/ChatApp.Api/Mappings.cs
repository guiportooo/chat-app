namespace ChatApp.Api
{
    using AutoMapper;
    using Requests = HttpIn.Requests;
    using Responses = HttpIn.Responses;
    using Commands = Domain.Commands;
    using Models = Domain.Models;

    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Requests.RegisterUser, Commands.RegisterUser>();
            CreateMap<Commands.RegisterUser, Models.User>();
            CreateMap<Models.User, Responses.User>();
        }
    }
}