using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using webapi.Dto;
using webapi.Hubs;

namespace Controllers.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> hub;
        public ChatController(IHubContext<ChatHub> hub)
        {
            this.hub = hub;
        }

        [HttpPost()]
        public ActionResult PostChat([FromBody] PostChatRequestDto data)
        {
            var result = hub.Clients.All.SendAsync(data.text, 5);
            if (!result.IsCompleted)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Post chat is error.");
            }
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}