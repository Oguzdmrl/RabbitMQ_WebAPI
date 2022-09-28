using Common.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RabbitMqApiController : ControllerBase
    {
        private readonly RabbitMQRepo _publisher;
        public RabbitMqApiController(RabbitMQRepo publisher) => _publisher = publisher;
        [HttpPost]
        public IActionResult PublishFanout([FromQuery] string message)
        {
            _publisher.PublishFanout(message);
            return Ok("FanoutExchange Mesaj gönderildi");
        }
        [HttpPost]
        public IActionResult PublishHeader([FromQuery] string args, string byteMessage)
        {
            _publisher.PublishHeader(args, byteMessage); // dotnet run 1
            return Ok("HeaderExchange Mesaj gönderildi");
        }
        [HttpPost]
        public IActionResult PublishDirect([FromQuery] string yetki, string byteMessage)
        {
            _publisher.PublishDirect(yetki, byteMessage); // dotnet run admin
            return Ok("DirectExchange Mesaj gönderildi");
        }
    }
}