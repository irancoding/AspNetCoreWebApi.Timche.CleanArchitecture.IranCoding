using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v2
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiVersion("2.0")]
    [ApiVersion("2.1",Deprecated =true)]
    [ApiVersion("2.2")]
    public class ApiVerController : ControllerBase
    {
        [HttpPost("{age}/{id}")]
        public IActionResult Create(int age, int id)
        {
            return Ok($"call from v2.0 age:{age} and id:{id}");
        } 
        
        [HttpPost]
        [MapToApiVersion("2.2")]
        [MapToApiVersion("2.1")]
        public IActionResult Test()
        {
            return Ok($"call from v2.0");
        }
    }
}
