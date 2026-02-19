using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TK_UR_BOOK.Domain.Sp_Interface;

namespace TK_UR_BOOK.Domain.Comman
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContext( IHttpContextAccessor httpContextAccessor)
        {
           _httpContextAccessor = httpContextAccessor; 
        }
        public UserId UserId => new UserId(Guid.Parse(
            _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value ?? 
            throw new UnauthorizedAccessException()));
           

        public string Email => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)!.Value?? string.Empty;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated;
    }
}
