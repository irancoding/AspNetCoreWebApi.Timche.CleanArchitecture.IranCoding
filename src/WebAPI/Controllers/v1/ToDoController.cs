using Application.Contracts;
using Application.Models.ToDos;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [CustomActionResultFilter]
    [ExceptionFilter]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoRepository _toDoRepository;
        public ToDoController(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateToDo([FromForm] ToDoDto dto)
        {
            var result = await _toDoRepository.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDoDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetToDo(Guid id)
        {
            var todo = await _toDoRepository.GetToDoById(id);
            return Ok(todo);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ToDoDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetToDoList()
        {
            var todos = await _toDoRepository.GetToDoListAsync();
            return Ok(todos);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateToDo(Guid id, [FromForm] ToDoDto dto)
        {
            await _toDoRepository.UpdateAsync(id,dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteToDo(Guid id)
        {
            var todo = await _toDoRepository.GetToDoById(id);
            await _toDoRepository.DeleteToDoAsync(todo);
            return Ok();
        }
    }
}
