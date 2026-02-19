using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Application.UseCases.Payment
{
    public record class ConfirmPurchaseCommand(UserId UserId,
    BookId BookId,
    string TransactionId,
    decimal Amount,
    string Currency);

}
