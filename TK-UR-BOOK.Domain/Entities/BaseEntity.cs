using TK_UR_BOOK.Domain.Sp_Interface;

namespace TK_UR_BOOK.Domain.Entities
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; protected set; } = default!;

    }

    public abstract class AuditTableEntity<TId> : BaseEntity<TId> , IAuditable
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }

        public bool? DeletedAt { get; set; } = false; //soft delete
    }
}
