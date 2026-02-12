using FluentValidation;
using TK_UR_BOOK.Domain.ValueObjects;

namespace TK_UR_BOOK.Application.Validations
{
    public class MoneyValidator : AbstractValidator<Money>
    {
        public MoneyValidator()
        {
            RuleFor(m => m.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
            

        }
    }
}
