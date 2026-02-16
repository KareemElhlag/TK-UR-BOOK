using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.UseCases.UserCommands;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IHashingPassword _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        public AuthService(IUnitOfWork unitOfWork,
            IEmailService emailService,
            IConfiguration configuration,
            IHashingPassword passwordHasher,
            IJwtProvider jwtProvider
            )
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        /// <summary>
        /// Let's implement the ChangePassword method. This method will take the user's email, 
        /// current password, and new password as parameters.
        /// It will first verify that the user exists and that the current password is correct.
        /// If both checks pass, it will update the user's password with the new one.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result<bool>> ChangePassword(string email, string currentPassword, string newPassword)
        {
            var user = await _unitOfWork.Repository<User>().GetFirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Result.Failure<bool>("User not found.");
            }
            if (!_passwordHasher.VerifyPassword(currentPassword, user.PasswordHash))
            {
                return Result.Failure<bool>("Current password is incorrect.");
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return Result.Failure<bool>("New password cannot be empty.");
            }
            user.UpdatePassword(_passwordHasher.HashPassword(newPassword));
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(true);
        }
        public async Task<Result<bool>> ConfirmEmail(ConfirmEmailCommand requst)
        {
            var CommandHandler = new ConfirmEmailCommandHandler(_unitOfWork);
            var result = await CommandHandler.Handle(requst);
            if (result.IsFailure)
            {
                return Result.Failure<bool>(result.Error);
            }
            return Result.Success<bool>(result.IsSuccess);

        }

        /// <summary>
        /// forgot password method will take the user's email as a parameter.
        /// It will check if the user exists and if so, it will generate a password reset token and send it to the user's email.
        /// The user can then use this token to reset their password.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result<bool>> ForgotPassword(string email)
        {
            var user = await _unitOfWork.Repository<User>().GetFirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Result.Failure<bool>("User not found.");
            }
            var token = RandomOTP().Value;
            await _emailService.SendEmailAsync(user.Email, "Password Reset", $"Your password reset token is: {token}");
            await _unitOfWork.Repository<OTPEntity>().AddAsync(new OTPEntity(user.Id, token));
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(true);

        }

        public Task<Result<string>> GenrateToken(User user)
        {
            var result = _jwtProvider.GenerateToken(user);
            if (string.IsNullOrWhiteSpace(result))
            {
                return Task.FromResult(Result.Failure<string>("Token is null"));
            }
            return Task.FromResult(Result.Success(result));
        }

        public async Task<Result<AuthResponseDto>> LogIn(LogInCommand command)
        {
            var CommandHandler = new LogInCommandHandler(_unitOfWork, _passwordHasher);
            var result = CommandHandler.Handle(command);
            // LogInCommandHandler will return Result<AuthResponseDto>
            if (result.IsFaulted)
            {
                return Result.Failure<AuthResponseDto>(result.Exception?.Message ?? "An error occurred while processing the login command.");
            }
            //Genrate token and refresh token
            var authResponse = result.Result;
            if (authResponse == null)
            {
                return Result.Failure<AuthResponseDto>("Invalid login attempt.");
            }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(authResponse.Value.UserId!.Value);
            var TokenResult = await GenrateToken(user!);
            if (TokenResult.IsFailure)
            {
                return Result.Failure<AuthResponseDto>(TokenResult.Error ?? "An error occurred while generating the token.");
            }
            authResponse.Value.Token = TokenResult.Value;
            var RefreshTokenResult = GenrateRefreshToken();
            var expiresIn = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:ExpireDays"] ?? "7")); // Default to 7 days if not configured
            if (RefreshTokenResult.IsFaulted)
            {
                return Result.Failure<AuthResponseDto>(RefreshTokenResult.Exception?.Message ?? "An error occurred while generating the refresh token.");
            }
            user!.AddRefreshToken(RefreshTokenResult.Result.Value, expiresIn);
            authResponse.Value.refreshToken = RefreshTokenResult.Result.Value;
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(authResponse.Value);
        }

        public async Task<Result> LogOut(string token)
        {
            var result = await RevokeToken(token, "User logged out");
            if (result.IsFailure)
            {
                return Result.Failure(result.Error ?? "An error occurred while logging out.");
            }
            return Result.Success();

        }

        private Task<Result<string>> GenrateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Task.FromResult(Result.Success(Convert.ToBase64String(randomBytes)));
        }
        public async Task<Result<AuthResponseDto>> RefreshAccesToken(string accesToken, string refreshToken)
        {
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(accesToken);
            if (principal == null)
            {
                return Result.Failure<AuthResponseDto>("Invalid access token.");
            }
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Result.Failure<AuthResponseDto>("User ID claim not found in token.");
            }
            var userId = Guid.Parse(userIdClaim.Value);
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
            if (user == null || !user.RefreshTokens.Any(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow))
            {
                return Result.Failure<AuthResponseDto>("Invalid refresh token.");
            }
            var newAccessToken = _jwtProvider.GenerateToken(user);
            var newRefreshToken = GenrateRefreshToken().Result.Value;
            user.AddRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:ExpireDays"] ?? "7")));
            _unitOfWork.SaveChangesAsync().Wait();
            var response = new AuthResponseDto
            {
                UserId = user.Id.Value,
                Email = user.Email,
                Token = newAccessToken,
                refreshToken = newRefreshToken
            };
            return Result.Success(response);
        }

        public async Task<Result<Guid>> RegisterNewUser(RegisterUserCommand command)
        {
            var CommandHandler = new RegisterUserCommandHandler(_unitOfWork, _passwordHasher, _emailService, _configuration);
            var result = await CommandHandler.Handler(command);
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }
            return Result.Success(result.Value);
        }
        /// <summary>
        /// ResendConfirmationEmail method will take the user's email as a parameter.
        /// It will check if the user exists and if their email is not already confirmed. 
        /// If both checks pass, it will generate a new email verification token and send it to the user's email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<Result<bool>> ResendConfirmationEmail(string email)
        {
            var user = await _unitOfWork.Repository<User>().GetFirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Result.Failure<bool>("User not found.");
            }
            if (user.IsEmailConfirmed)
            {
                return Result.Failure<bool>("Email is already confirmed.");
            }
            user.GenerateEmailVerificationToken();
            await _unitOfWork.SaveChangesAsync();
            var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "http://localhost:5000";
            var verificationLink = $"{baseUrl}/api/auth/confirm-email?token={user.EmailVerificationToken}&email={user.Email}";
            await _emailService.SendEmailAsync(user.Email, "Email Confirmation", $"Please confirm your email by clicking on the following link: {verificationLink}");
            return Result.Success(true);
        }

        /// <summary>
        /// ResetPassword method will take the user's email, password reset token, and new password as parameters.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result<bool>> ResetPassword(string email, string token, string newPassword)
        {
            var user = await _unitOfWork.Repository<User>().GetFirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Result.Failure<bool>("User not found.");
            }
            var otpEntity = await _unitOfWork.Repository<OTPEntity>().GetFirstOrDefaultAsync(o => o.userId == user.Id && o.code == token);
            if (otpEntity == null || !otpEntity.IsValid)
            {
                return Result.Failure<bool>("Invalid or expired token.");
            }
            user.UpdatePassword(_passwordHasher.HashPassword(newPassword));
            otpEntity.MarkAsUsed();
            _unitOfWork.Repository<OTPEntity>().Update(otpEntity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(true);
        }

        private async Task<Result<bool>> RevokeToken(string Token, string reson)
        {
            var refreshToken = await _unitOfWork.Repository<RefreshToken>().GetFirstOrDefaultAsync(rt => rt.Token == Token);
            if (refreshToken == null)
            {
                return Result.Failure<bool>("Refresh token not found.");
            }
            refreshToken.Revoke(reson);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(true);
        }
        /// <summary>
        /// for security reasons,
        /// we should implement the UpdateRefreshToken 
        /// method to allow users to revoke their refresh tokens when 
        /// they log out or when they suspect that their token has been compromised.
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<Result<RefreshToken>> UpdateRefreshToken(RefreshToken Token)
        {
            var result = RevokeToken(Token.Token, "Token updated");
            if (result.IsFaulted)
            {
                return Result.Failure<RefreshToken>(result.Result.Error ?? "An error occurred while updating the refresh token.");
            }
            var newRefreshTokenResult = GenrateRefreshToken();
            if (newRefreshTokenResult != null)
            {
                var newRefreshToken = RefreshToken
                    .CreateNew(Token.UserId, newRefreshTokenResult.Result.Value,
                    DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:ExpireDays"] ?? "7")));
            }
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(Token);
        }
        

        private Result<string> RandomOTP()
        {
            var randomBytes = new byte[6];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            var otp = BitConverter.ToUInt32(randomBytes, 0) % 1000000; // Generate a 6-digit OTP
            return Result.Success(otp.ToString("D6"));
        }
    }
}
