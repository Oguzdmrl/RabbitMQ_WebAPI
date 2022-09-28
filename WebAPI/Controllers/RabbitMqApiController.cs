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
    }
}