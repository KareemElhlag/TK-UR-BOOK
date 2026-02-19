using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Application.UseCases.RatingBook
{
    public sealed record CreateRetingCommand(UserId userId,
            BookId bookId,
            int star,
            string? comment);
    
    
}
