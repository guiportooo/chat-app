namespace ChatApp.Api.HttpIn.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Commands = Domain.Commands;
    using Queries = Domain.Queries;

    [ApiController]
    [Authorize]
    [Route("rooms/{roomCode}/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public MessagesController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetMessage")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string roomCode, int id)
        {
            var query = new Queries.GetMessageById(id);
            var message = await _mediator.Send(query);

            if (message is null)
                return NotFound();

            var response = _mapper.Map<Responses.Message>(message);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Responses.MessageSent))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string roomCode)
        {
            var query = new Queries.GetLastFiftyMessagesByRoom(roomCode);
            var messages = await _mediator.Send(query);

            if (!messages.Any())
                return NoContent();

            var response = new Responses.MessagesSent(messages);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Responses.MessageSent))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Responses.Error))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post(string roomCode, [FromBody] Requests.SendMessage request)
        {
            var userName = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(userName))
                return Unauthorized();

            var command = new Commands.SendMessage(roomCode, userName, request.Text);
            var message = await _mediator.Send(command);

            if (message is null)
                return Accepted();

            var response = _mapper.Map<Responses.MessageSent>(message, opt =>
                opt.AfterMap((_, dest) =>
                {
                    dest.UserName = userName;
                    dest.RoomCode = roomCode;
                }));
            return CreatedAtRoute("GetMessage", new { roomCode, id = message.Id }, response);
        }
    }
}