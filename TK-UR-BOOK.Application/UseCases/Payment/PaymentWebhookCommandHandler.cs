using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.UseCases.Purchasing;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;
using TK_UR_BOOK.Domain.Enums;
using TK_UR_BOOK.Domain.ValueObjects;

namespace TK_UR_BOOK.Application.UseCases.Payment
{
    public class PaymentWebhookCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreatePurchaseCommandHandler _createPurchaseCommandHandler;
        public PaymentWebhookCommandHandler(IUnitOfWork unitOfWork
            , CreatePurchaseCommandHandler createPurchaseCommandHandler)
        {
            _unitOfWork = unitOfWork;
            _createPurchaseCommandHandler = createPurchaseCommandHandler;
        }
        public async Task<Result> Handler(ConfirmPurchaseCommand command)
        {
            var isAlreadyProsses = await _unitOfWork.Repository<Purchase>().GetFirstOrDefaultAsync(P => P.TransactionId == command.TransactionId);
            if (isAlreadyProsses != null)
                return Result.Success();

            if (!Enum.TryParse<Currency>(command.Currency, true, out var parsedCurrency))
            {
                return Result.Failure("Currency not supported.");
            }

            var paidAmount = new Money(command.Amount, parsedCurrency);

            var purchase = await _createPurchaseCommandHandler.Handler(new CreateParchaseCommand(
                command.UserId,
                command.BookId,
                paidAmount,
                command.TransactionId));


            var activity = new UserActivity(
                command.UserId,
            command.BookId,
            ActivationType.PurchaseConfirmed,
            $"Transaction: {command.TransactionId}"
                );
            await _unitOfWork.Repository<UserActivity>().AddAsync(activity);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();

        }
    }
}
