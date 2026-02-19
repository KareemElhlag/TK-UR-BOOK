using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Domain.Sp_Interface
{
    public interface IUserContext
    {
        UserId UserId { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
    }
}
