using Microsoft.EntityFrameworkCore;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.UseCases.Purchasing
{
    public class GetBookAllPurchaseQureyHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBookAllPurchaseQureyHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public  async Task<Result<List<Purchase>>> Handler(GetBookAllPurchaseQurey query)
        {
            var purchases = await _unitOfWork.Repository<Purchase>().GetQueryable(p => p.BookId == query.bookId).ToListAsync();
            return Result.Success(purchases);
            ;
        }
    }
}
