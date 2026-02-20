using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.UseCases.RatingBooks
{
    public class DeletRatingCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeletRatingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handler( DeletRatingCommand command)
        {
            var result = await _unitOfWork.Repository<Rating>().GetByIdAsync(command.RatingId);
            if (result == null)
                return Result.Failure("Rating not found ");
             _unitOfWork.Repository<Rating>().Remove(result);
            var userActivity = new UserActivity
                (
                result.UserId,
                result.BookId,
                Domain.Enums.ActivationType.deletRating,
                $"Your Rating {result.Id} was deleted by {result.UserId}"
                );

            return Result.Success();
        }
    }
}
