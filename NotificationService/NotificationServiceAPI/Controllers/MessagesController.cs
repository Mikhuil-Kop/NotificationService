using Microsoft.AspNetCore.Mvc;

namespace NotificationServiceAPI.Controllers
{
    /// <summary>
    /// Контроллер для отправки сообщений пользователям через HTTP.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        [HttpPost("SendMessageToEveryone")]
        public async Task<IActionResult> SendMessageToEveryoneAsync(string message)
        {

        }
    }
}
