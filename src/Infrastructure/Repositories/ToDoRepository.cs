using Application.Exceptions;
using Application.Models.ToDos;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<ToDoDto> _validator;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private const string ImagePath = "images/todo";
        private string uploadRootpath = "";
        public ToDoRepository(ApplicationDbContext dbContext, IValidator<ToDoDto> validator, IMapper mapper, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _validator = validator;
            _mapper = mapper;
            _environment = environment;
            var uploadRootpath = Path.Combine(_environment.WebRootPath, ImagePath);
            if (!Directory.Exists(uploadRootpath))
            {
                Directory.CreateDirectory(uploadRootpath);
            }
        }

        public async Task<Guid> CreateAsync(ToDoDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new CustomValidationException(validationResult.Errors);
            }
            var uploadRootpath = Path.Combine(_environment.WebRootPath, ImagePath);
            var todo = _mapper.Map<ToDo>(dto);
            if (dto.ImageFile is not null && dto.ImageFile.Length > 0)
            {
                string fileExtension = Path.GetExtension(Path.GetFileName(dto.ImageFile.FileName));
                string newFileName = $"todo_{Guid.NewGuid().ToString().Replace("-", "")}{fileExtension}";
                var filePath = Path.Combine(uploadRootpath, newFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await dto.ImageFile.CopyToAsync(fileStream).ConfigureAwait(false);
                todo.ImagePath = $"/{ImagePath}/{newFileName}";
            }
            await _dbContext.ToDos.AddAsync(todo);
            await _dbContext.SaveChangesAsync();
            return todo.Id;
        }

        public async Task DeleteToDoAsync(ToDoDto dto)
        {
            var todo = _mapper.Map<ToDo>(dto);
            _dbContext.Remove(todo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ToDoDto> GetToDoById(Guid id)
        {
            var todo = await _dbContext.ToDos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (todo == null)
            {
                throw new NotFoundException(nameof(ToDo), id);
            }
            return _mapper.Map<ToDoDto>(todo);
        }

        public async Task<List<ToDoDto>> GetToDoListAsync()
        {
            var todos = await _dbContext.ToDos.AsNoTracking().ToListAsync();
            return _mapper.Map<List<ToDoDto>>(todos);
        }

        public async Task UpdateAsync(Guid id, ToDoDto dto)
        {
            if (id != dto.Id)
            {
                throw new CustomException("شناسه ارسالی با شناسه مدل یکی نمی باشد");
            }
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new CustomValidationException(validationResult.Errors);
            }
            var uploadRootpath = Path.Combine(_environment.WebRootPath, ImagePath);
            var mainTodo = await _dbContext.ToDos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (mainTodo == null)
            {
                throw new NotFoundException(nameof(ToDo), id);
            }
            var todo = _mapper.Map<ToDo>(dto);
            if (dto.ImageFile is not null && dto.ImageFile.Length > 0)
            {
                string fileExtension = Path.GetExtension(Path.GetFileName(dto.ImageFile.FileName));
                string newFileName = $"todo_{Guid.NewGuid().ToString().Replace("-", "")}{fileExtension}";
                var filePath = Path.Combine(uploadRootpath, newFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await dto.ImageFile.CopyToAsync(fileStream).ConfigureAwait(false);
                todo.ImagePath = $"/{ImagePath}/{newFileName}";
                if (!string.IsNullOrWhiteSpace(mainTodo.ImagePath))
                {
                    DeleteImage(mainTodo.ImagePath);
                }
            }
            else
            {
                todo.ImagePath = mainTodo.ImagePath;
            }
            var entity = _dbContext.Entry(todo);
            entity.State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        private void DeleteImage(string path)
        {
            var imagePath = _environment.WebRootPath + path;
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}
