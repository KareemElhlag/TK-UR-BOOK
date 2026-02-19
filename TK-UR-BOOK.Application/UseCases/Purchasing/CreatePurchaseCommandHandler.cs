using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.UseCases.Purchasing
{
    public  class CreatePurchaseCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
       public CreatePurchaseCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        
        public async Task<Result> Handler(CreateParchaseCommand command)
        {
            var purchase = new Purchase(
                 command.userId,
                 command.bookId,
                 command.paidAmount,
                 command.TransactionId
                );
            await _unitOfWork.Repository<Domain.Entities.Purchase>().AddAsync(purchase);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
