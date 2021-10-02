namespace ChatApp.Api.Domain.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using HttpIn.Authentication;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public record AuthenticateUser(string UserName, string Password) : IRequest<string>;

    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUser, string>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<AuthenticateUserHandler> _logger;

        public AuthenticateUserHandler(IMapper mapper,
            IMediator mediator,
            ITokenGenerator tokenGenerator,
            ILogger<AuthenticateUserHandler> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        public async Task<string> Handle(AuthenticateUser request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<Queries.GetUserByUserNameAndPassword>(request);
            var user = await _mediator.Send(query, cancellationToken);

            if (user is null)
                return string.Empty;

            var token = _tokenGenerator.Generate(user);
            _logger.LogInformation("User {UserName} authenticated", user.UserName);
            return token;
        }
    }
}