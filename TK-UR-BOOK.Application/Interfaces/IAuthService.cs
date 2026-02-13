using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.UseCases.UserCommands;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<Guid>> RegisterNewUser(RegisterUserCommand command);
        Task<Result<bool>> ChangePassword(string email, string currentPassword, string newPassword);
        Task<Result<bool>> ForgotPassword(string email);
        Task<Result<bool>> ResetPassword(string email, string token, string newPassword);
        Task<Result<bool>> ConfirmEmail(ConfirmEmailCommand requst);
        Task<Result<bool>> ResendConfirmationEmail(string email);
        Task<Result<AuthResponseDto>> LogIn(LogInCommand command);
        Task<Result<string>> GenrateToken(User user);
        Task<Result<AuthResponseDto>> RefreshToken(string expiredToken, string refreshToken);
        Task<Result<bool>> RevokeToken(string Token);
        Task<Result<RefreshToken>> UpdateRefreshToken(RefreshToken Token);

        Task LogOut();
    }
}
