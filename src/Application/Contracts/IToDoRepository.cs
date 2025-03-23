using Application.Models.ToDos;
using Domain.Entities;

namespace Application.Contracts
{
    public interface IToDoRepository
    {
        Task<Guid> CreateAsync(ToDoDto dto);
        Task UpdateAsync(Guid id,ToDoDto dto);
        Task<ToDoDto> GetToDoById(Guid id);
        Task<List<ToDoDto>> GetToDoListAsync();
        Task DeleteToDoAsync(ToDoDto dto);
    }
}
