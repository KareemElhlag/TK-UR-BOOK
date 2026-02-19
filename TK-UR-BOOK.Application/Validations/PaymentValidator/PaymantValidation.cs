using FluentValidation;
using TK_UR_BOOK.Application.UseCases.Payment;

namespace TK_UR_BOOK.Application.Validations.PaymentValidator
{
    public class PaymantValidation : AbstractValidator<PaymentCommand>
    {
        public PaymantValidation()
        {
            RuleFor(p => p.UserId).NotNull().WithMessage(" UserId Not found ");
            RuleFor(p => p.BookId).NotNull().WithMessage("BookId Not found");
            RuleFor(Q => Q.quantity).NotNull().LessThanOrEqualTo(0).WithMessage(" Quantity must be greter tahn zero ");  
                }
    }
}
