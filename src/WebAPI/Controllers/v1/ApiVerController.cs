using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ApiVerController : ControllerBase
    {
        [HttpPost("{age}")]
        public IActionResult Create(int age)
        {
            return Ok($"call from v1.0 age:{age}");
        }
    }
}
