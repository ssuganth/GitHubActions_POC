using FluentValidation;

namespace SampleApp.Application.Category.Commands.Create
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(v => v.Description)
                .MaximumLength(150)
                .NotNull()
                .WithMessage("Description cannot be empty");

            RuleFor(v => v.Name)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage("Name cannot be empty");
        }
    }
}