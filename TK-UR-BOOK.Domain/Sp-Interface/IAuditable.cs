
namespace TK_UR_BOOK.Domain.Sp_Interface
{
    public interface IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
