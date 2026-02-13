using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.Validations.CommandValidator;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.UseCases.UserCommands
{
    public class LogInCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashingPassword _passwordHasher;

        public LogInCommandHandler(IUnitOfWork unitOfWork, IHashingPassword hashingPassword)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = hashingPassword;
        }
        public async Task<Result<AuthResponseDto>> Handle(LogInCommand command)
        {
            var validator = new LogInCommandValidator();
            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure<AuthResponseDto>(errors);
            }
            var userRepository = _unitOfWork.Repository<User>();
            var user = await userRepository.GetFirstOrDefaultAsync(u => u.Email == command.Email);
            if (user == null || !_passwordHasher.VerifyPassword(command.Password, user.PasswordHash))
            {
                return Result.Failure<AuthResponseDto>("Invalid email or password.");
            }
            if (!user.IsEmailConfirmed)
            {
                return Result.Failure<AuthResponseDto>("Email not confirmed.");
            }
            var response = new AuthResponseDto
            {
                UserId = user.Id.Value,
                Email = user.Email
            };
            return Result.Success(response);
        }
    }
}
