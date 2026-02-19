using Microsoft.EntityFrameworkCore;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.UseCases.Purchasing
{
    public class GetUserAllPurchaseQureyHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserAllPurchaseQureyHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<Result<List<Purchase>>> Handler(GetUserAllPurchaseQurey query)
        {
            var purchases = await _unitOfWork.Repository<Purchase>().GetQueryable(p => p.UserId == query.userId).ToListAsync();
            return Result.Success(purchases);
            ;
        }
    }
}
