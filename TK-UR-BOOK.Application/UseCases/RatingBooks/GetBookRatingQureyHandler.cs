using Microsoft.EntityFrameworkCore;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.UseCases.RatingBooks
{
    public class GetBookRatingQureyHandler
    {
        private readonly IUnitOfWork _untiOfWork;
        public GetBookRatingQureyHandler(IUnitOfWork unitOfWork)
        {
            _untiOfWork = unitOfWork;
        }

        public async Task<Result<List<Rating>>> Handler(GetBookRatingQurey qurey)
        {
            var qureyList = await _untiOfWork.Repository<Rating>()
                .GetQueryable(r => r.BookId == qurey.BookId).ToListAsync();
            if (qureyList == null)
                return Result.Failure<List<Rating>>("Not Found Rating");
            return Result.Success(qureyList);
        }
    }
}
