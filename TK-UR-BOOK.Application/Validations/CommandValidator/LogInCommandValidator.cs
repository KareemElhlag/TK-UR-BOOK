using FluentValidation;
using TK_UR_BOOK.Application.UseCases.UserCommands;

namespace TK_UR_BOOK.Application.Validations.CommandValidator
{
    public class LogInCommandValidator : AbstractValidator<LogInCommand>
    {
        public LogInCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
