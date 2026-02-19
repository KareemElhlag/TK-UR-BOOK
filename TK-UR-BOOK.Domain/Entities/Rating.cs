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
            string? comment
            )
        {
            if (star < 1 || star > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(star), "Star rating must be between 1 and 5.");
            }
            UserId = userId;
            BookId = bookId;
            StarRating = star;
            Comment = comment;
        }


    }
}
