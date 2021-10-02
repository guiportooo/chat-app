namespace ChatApp.Api.HttpIn.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Commands = Domain.Commands;
    using Queries = Domain.Queries;

    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UsersController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("users/{id:int}", Name = "Get")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Responses.User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var query = new Queries.GetUserById(id);
            var user = await _mediator.Send(query);

            if (user is null)
                return NotFound();

            var response = _mapper.Map<Responses.User>(user);
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Responses.User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Responses.Error))]
        public async Task<IActionResult> Register([FromBody] Requests.RegisterUser request)
        {
            var command = _mapper.Map<Commands.RegisterUser>(request);
            var user = await _mediator.Send(command);
            var response = _mapper.Map<Responses.User>(user);
            return CreatedAtRoute(nameof(Get), new { id = user.Id }, response);
        }

        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Responses.AuthenticatedUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Authenticate([FromBody] Requests.AuthenticateUser request)
        {
            var command = _mapper.Map<Commands.AuthenticateUser>(request);
            var token = await _mediator.Send(command);

            if (string.IsNullOrWhiteSpace(token))
                return NotFound();

            var response = new Responses.AuthenticatedUser(request.UserName, token);
            return Ok(response);
        }
    }
}