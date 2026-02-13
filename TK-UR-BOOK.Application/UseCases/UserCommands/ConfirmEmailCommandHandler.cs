using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.UseCases.UserCommands
{
    public  class ConfirmEmailCommandHandler 
    {
        private readonly IUnitOfWork _unitOfWork;
        public  ConfirmEmailCommandHandler( IUnitOfWork unitOfWork)=> _unitOfWork = unitOfWork;

        public async Task<Result> Handle(ConfirmEmailCommand command)
        {
            var userRepository = _unitOfWork.Repository<User>();
            var user = await userRepository.GetFirstOrDefaultAsync(u => u.Email == command.Email
            && u.EmailVerificationToken == command.Token );
            if (user == null || user.EmailVerificationToken != command.Token)
            {
                return Result.Failure("Invalid email or token.");
            }
            user.ConfirmEmail();
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }



    }
}
