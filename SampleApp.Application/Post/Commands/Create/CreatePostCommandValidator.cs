using FluentValidation;

namespace SampleApp.Application.Post.Commands.Create
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(v => v.CategoryId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Category cannot be empty or empty");

            RuleFor(v => v.Content)
                .MaximumLength(100)
                .NotEmpty()
                .WithMessage("Content is not proper for post");

            RuleFor(v => v.Title)
                .MaximumLength(25)
                .NotEmpty()
                .WithMessage("Title is not proper for post");
        }
    }
}