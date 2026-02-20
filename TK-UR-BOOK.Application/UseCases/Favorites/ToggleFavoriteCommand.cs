using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Application.UseCases.Favorites
{
    public sealed record ToggleFavoriteCommand(UserId UserId , BookId BookId);
    
}
