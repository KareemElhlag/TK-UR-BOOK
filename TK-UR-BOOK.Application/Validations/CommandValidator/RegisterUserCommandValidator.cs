using FluentValidation;
using TK_UR_BOOK.Application.UseCases.UserCommands;

namespace TK_UR_BOOK.Application.Validations.CommandValidator
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");   

        }
    }
}
