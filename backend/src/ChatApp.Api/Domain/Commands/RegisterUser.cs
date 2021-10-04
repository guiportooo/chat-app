namespace ChatApp.Api.Domain.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Exceptions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Models;
    using Repositories;
    using Services;

    public record RegisterUser(string UserName, string Password) : IRequest<User>;

    public class RegisterUserHandler : IRequestHandler<RegisterUser, User>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(IUserRepository repository,
            IPasswordHasher passwordHasher,
            ILogger<RegisterUserHandler> logger)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<User> Handle(RegisterUser command, CancellationToken cancellationToken)
        {
            var (userName, password) = command;
            var existingUser = await _repository.GetByUserName(userName);

            if (existingUser is not null)
                throw new UserNameNotAvailableException(userName);

            var protectedPassword = _passwordHasher.Hash(password);
            var user = new User(userName, protectedPassword);
            await _repository.Add(user);
            _logger.LogInformation("New user {UserName} registered", userName);
            return user;
        }
    }
}