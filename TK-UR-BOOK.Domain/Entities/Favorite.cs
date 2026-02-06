using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Domain.Entities
{
    public class Favorite : BaseEntity<long>
    {
        public UserId UserId { get; private set; }
        public BookId BookId { get; private set; }
        public DateTime AddedAt { get; private set; } = DateTime.UtcNow;
        public Favorite(
            UserId userId
            , BookId bookId
            )
        {
            UserId = userId;
            BookId = bookId;
        }
    }
}
