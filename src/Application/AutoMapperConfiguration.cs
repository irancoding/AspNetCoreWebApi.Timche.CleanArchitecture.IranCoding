using Application.Models.ToDos;
using AutoMapper;
using Domain.Entities;

namespace Application
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<ToDoDto, ToDo>().ReverseMap();
        }
    }
}
