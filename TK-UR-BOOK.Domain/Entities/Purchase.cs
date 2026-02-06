using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.ValueObjects;

namespace TK_UR_BOOK.Domain.Entities
{
    public class Purchase : AuditTableEntity<Guid>
    {
        public UserId UserId { get; private set; }
        public BookId BookId { get; private set; }

        public Money PaidAmount { get; private set; }

        public string TransactionId { get; private set; } 


        public Purchase(UserId userId, BookId bookId, Money paidAmount, string transactionId)
        {
            UserId = userId;
            BookId = bookId;
            PaidAmount = paidAmount;
            TransactionId = transactionId;
        }

    }
}
