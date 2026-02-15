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
        public Task<Result<bool>> ChangePassword(string email, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
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

        public Task<Result<bool>> ForgotPassword(string email)
        {
            throw new NotImplementedException();
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
        public Task<Result<AuthResponseDto>> RefreshToken(string accesToken, string refreshToken)
        {
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(accesToken);
            if (principal == null)
            {
                return Task.FromResult(Result.Failure<AuthResponseDto>("Invalid access token."));
            }
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Task.FromResult(Result.Failure<AuthResponseDto>("User ID claim not found in token."));
            }
            var userId = Guid.Parse(userIdClaim.Value);
            var user = _unitOfWork.Repository<User>().GetByIdAsync(userId).Result;
            if (user == null || !user.RefreshTokens.Any(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow))
            {
                return Task.FromResult(Result.Failure<AuthResponseDto>("Invalid refresh token."));
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
            return Task.FromResult(Result.Success(response));
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

        public Task<Result<bool>> ResendConfirmationEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> ResetPassword(string email, string token, string newPassword)
        {
            throw new NotImplementedException();
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

        public Task<Result<RefreshToken>> UpdateRefreshToken(RefreshToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
