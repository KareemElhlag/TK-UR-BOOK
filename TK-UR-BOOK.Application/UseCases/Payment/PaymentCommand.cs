using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Application.UseCases.Payment
{
    public sealed  record PaymentCommand(BookId BookId , UserId UserId , int quantity);
    
    
}
