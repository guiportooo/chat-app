namespace ChatApp.Api.Domain.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using HttpIn.Authentication;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Repositories;
    using Services;

    public record AuthenticateUser(string UserName, string Password) : IRequest<string>;

    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUser, string>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<AuthenticateUserHandler> _logger;

        public AuthenticateUserHandler(IUserRepository repository,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            ILogger<AuthenticateUserHandler> logger)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        public async Task<string> Handle(AuthenticateUser command, CancellationToken cancellationToken)
        {
            var (userName, password) = command;
            var user = await _repository.GetByUserName(userName);

            if (user is null)
                return string.Empty;

            if (!_passwordHasher.Check(user.Password, password))
                return string.Empty;

            var token = _tokenGenerator.Generate(user);
            _logger.LogInformation("User {UserName} authenticated", user.UserName);
            return token;
        }
    }
}