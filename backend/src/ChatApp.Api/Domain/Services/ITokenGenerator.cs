namespace ChatApp.Api.Domain.Services
{
    using Models;

    public interface ITokenGenerator
    {
        string Generate(User user);
    }
}