using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace TK_UR_BOOK.Application.UseCases.UserCommands
{
    public class RegisterUserCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashingPassword _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public RegisterUserCommandHandler(IUnitOfWork unitOfWork,
            IHashingPassword passwordHasher, 
            IEmailService emailService,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<Result<Guid>> Handler (RegisterUserCommand requst)
        {
            var uesrExist = await _unitOfWork.Repository<User>().GetFirstOrDefaultAsync(u => u.Email == requst.Email);
            if (uesrExist is not null)
            {
                return Result.Failure<Guid>("Email already exists");
            }
            var hashedPassword = _passwordHasher.HashPassword(requst.Password);
            var userId = new UserId(Guid.NewGuid());
            var user = new User(userId, requst.Name, requst.Email, hashedPassword, requst.PhoneNumber);
            user.GenerateEmailVerificationToken();
            if (requst.GroupId is not null)
            {
                var group = await _unitOfWork.Repository<Group>().GetByIdAsync(requst.GroupId.Value);
                if (group is null)
                {
                     group = await _unitOfWork.Repository<Group>().GetFirstOrDefaultAsync(g => g.Name == "User");
                    user.AddToGroup(group!);
                }
                else
                {
                    return Result.Failure<Guid>(" We can not add prmations...");
                }
            }

            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "http://localhost:5000";
            var verificationLink = $"{baseUrl}/api/auth/confirm-email?token={user.EmailVerificationToken}&email={user.Email}";
            await _emailService.SendEmailAsync(user.Email, "Email Confirmation", $"Please confirm your email by clicking on the following link: {verificationLink}");
            return Result.Success(userId.Value);

        }
    }
}
