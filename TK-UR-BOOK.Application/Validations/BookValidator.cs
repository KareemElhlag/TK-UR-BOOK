using FluentValidation;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.Validations
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(b => b.Price).SetValidator(new MoneyValidator());
            RuleFor(b => b.Author)
               .NotEmpty().WithMessage("Author is required.");
      
        }
    }
}
