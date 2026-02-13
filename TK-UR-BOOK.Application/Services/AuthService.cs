using Microsoft.Extensions.Configuration;
using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.UseCases.UserCommands;
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
        public AuthService(IUnitOfWork unitOfWork,
            IEmailService emailService,
            IConfiguration configuration,
            IHashingPassword passwordHasher
            )
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }
        public Task<Result<bool>> ChangePassword(string email, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public  async Task<Result<bool>> ConfirmEmail(ConfirmEmailCommand requst)
        {
            var CommandHandler =new ConfirmEmailCommandHandler(_unitOfWork);
            var result = await CommandHandler.Handle(requst);
            if (result.IsFailure)
            {
                return Result.Failure<bool>(result.Error);
            }
            return Result.Success<bool>(result.IsSuccess) ;

        }

        public Task<Result<bool>> ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> GenrateToken(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Result<AuthResponseDto>> LogIn(LogInCommand command)
        {
            var CommandHandler = new LogInCommandHandler(_unitOfWork, _passwordHasher);
            var result = CommandHandler.Handle(command);
            // LogInCommandHandler will return Result<AuthResponseDto>
            if (result.IsFaulted)
            {
                return Task.FromResult(Result.Failure<AuthResponseDto>(result.Exception?.Message ?? "An error occurred while processing the login command."));
            }
            //Genrate token and refresh token

            return result;
        }

        public Task LogOut()
        {
            throw new NotImplementedException();
        }

        public Task<Result<AuthResponseDto>> RefreshToken(string expiredToken, string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Guid>> RegisterNewUser(RegisterUserCommand command)
        {
            var CommandHandler = new RegisterUserCommandHandler(_unitOfWork ,_passwordHasher,_emailService,_configuration);
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

        public Task<Result<bool>> RevokeToken(string Token)
        {
            throw new NotImplementedException();
        }

        public Task<Result<RefreshToken>> UpdateRefreshToken(RefreshToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
