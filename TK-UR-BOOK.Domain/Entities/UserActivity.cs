using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Domain.Entities
{
    public class UserActivity : BaseEntity<long>
    {
        public UserId UserId { get; private set; }
        public BookId BookId { get; private set; }

        public string ActivationType { get; private set; }

        public string? MetaData { get; private set; }

        public DateTime? Timestamp { get; private set; } = DateTime.UtcNow;

        public UserActivity(
            UserId userId,
            BookId bookId
            , string activationType
            , string? metaData
            )
        {
            UserId = userId;
            BookId = bookId;
            ActivationType = activationType;
            MetaData = metaData;

        }

    }
}
