using Application.Models.ToDos;
using FluentValidation;

namespace Application.Validators.ToDoValidators
{
    public class TodoDtoValidator : AbstractValidator<ToDoDto>
    {
        public TodoDtoValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Title)
                .MaximumLength(100)
                .NotEmpty()
                .WithName("عنوان");

            RuleFor(x => x.Description)
                .MaximumLength(300)
                .WithName("توضیحات");
        }
    }
}
