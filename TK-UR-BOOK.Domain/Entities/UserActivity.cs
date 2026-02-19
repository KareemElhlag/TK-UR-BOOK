using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Enums;

namespace TK_UR_BOOK.Domain.Entities
{
    public class UserActivity : BaseEntity<long>
    {
        public UserId UserId { get; private set; }
        public BookId BookId { get; private set; }

        public ActivationType ActivationType { get; private set; }

        public string? MetaData { get; private set; }

        public DateTime? Timestamp { get; private set; } = DateTime.UtcNow;
        private UserActivity() { }

        public UserActivity(
            UserId userId,
            BookId bookId
            , ActivationType activationType
            , string? metaData
            )
        {
            UserId = userId;
            BookId = bookId;
            ActivationType = activationType;
            MetaData = metaData;

        }

        public static UserActivity Create(
            UserId userId,
            BookId bookId
            , ActivationType activationType
            , string? metaData
            )
        {
            return new UserActivity(userId, bookId, activationType, metaData);

        }
    }
}
