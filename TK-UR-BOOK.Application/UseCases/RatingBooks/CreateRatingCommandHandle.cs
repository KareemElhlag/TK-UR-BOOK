using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.UseCases.RatingBook;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;
using TK_UR_BOOK.Domain.Enums;

namespace TK_UR_BOOK.Application.UseCases.RatingBooks
{
    public class CreateRatingCommandHandle
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateRatingCommandHandle(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handler(CreateRetingCommand command)
        {
            try
            {
                var rating = new Rating(
                    command.userId,
                    command.bookId,
                    command.star,
                    command.comment
                );
                var UserActivity = new UserActivity(
                    command.userId,
                    command.bookId,
                    ActivationType.rating,
                    $"Rated book with ID: {command.bookId} with {command.star} stars."
                );
                await _unitOfWork.Repository<UserActivity>().AddAsync(UserActivity);
                await _unitOfWork.Repository<Rating>().AddAsync(rating);
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
