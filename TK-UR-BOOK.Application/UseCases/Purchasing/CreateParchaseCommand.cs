using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.ValueObjects;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TK_UR_BOOK.Application.UseCases.Purchasing
{
    public sealed record  CreateParchaseCommand(UserId userId,
                 BookId bookId,
                 Money paidAmount,
                 string TransactionId);
   
}
