using System.Security.Claims;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
        ClaimsPrincipal GetPrincipalFromExpiredToken (string token);
    }
}
