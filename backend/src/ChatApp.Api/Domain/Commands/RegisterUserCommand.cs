namespace ChatApp.Api.Domain.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Exceptions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Models;
    using Repositories;

    public record RegisterUser(string UserName, string Password) : IRequest<User>;

    public class RegisterUserHandler : IRequestHandler<RegisterUser, User>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(IUserRepository repository, IMapper mapper, ILogger<RegisterUserHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<User> Handle(RegisterUser command, CancellationToken cancellationToken)
        {
            var (userName, _) = command;
            var existingUser = await _repository.GetByUserName(userName);

            if (existingUser is not null)
                throw new UserNameNotAvailableException(userName);

            var user = _mapper.Map<User>(command);
            await _repository.Add(user);
            _logger.LogInformation("New user {UserName} registered", userName);
            return user;
        }
    }
}