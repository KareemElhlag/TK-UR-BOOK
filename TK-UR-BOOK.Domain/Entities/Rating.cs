using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Domain.Entities
{
    public class Rating : AuditTableEntity<long>
    {
        public UserId UserId { get; private set; }
        public BookId BookId { get; private set; }

        public int StarRating { get; private set; }

        public string? Comment { get; private set; }

        public DateTime RatedAt { get; private set; } = DateTime.UtcNow;
        private Rating() { }

        public Rating(
            UserId userId,
            BookId bookId,
            int star,
            string comment
            )
        {
            UserId = userId;
            BookId = bookId;
            StarRating = star;
            Comment = comment;
        }
    }
}
