using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChatApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private readonly ILogger<HelloWorldController> _logger;

        public HelloWorldController(ILogger<HelloWorldController> logger) => _logger = logger;

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("[ChatApp.Api] Hello World!");
            return Ok("Hello World!");
        }
    }
}
