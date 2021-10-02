namespace ChatApp.Api.HttpIn.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Commands = Domain.Commands;

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
            var query = new GetMessageById(id);
            var message = await _mediator.Send(query);

            if (message is null)
                return NotFound();

            var response = _mapper.Map<Responses.Message>(message);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Responses.MessageSent))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Responses.Error))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post(string roomCode, [FromBody] Requests.SendMessage request)
        {
            var userName = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(userName))
                return Unauthorized();

            var command = new Commands.SendMessage(roomCode, userName, request.Text);
            var message = await _mediator.Send(command);
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