using Microsoft.EntityFrameworkCore;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.UseCases.Favorites
{
    public class ToggleFavoriteCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public ToggleFavoriteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ToggleFavoriteCommand command, CancellationToken ct)
        {
            var favRepo = _unitOfWork.Repository<Favorite>();
            var existingFav = await favRepo.GetQueryable(q => q.UserId == command.UserId && q.BookId == command.BookId)
                .FirstOrDefaultAsync(ct);
            if (existingFav != null)
            {

                favRepo.Remove(existingFav);
            }
            else
            {
                var favorite = new Favorite(command.UserId, command.BookId);

                await favRepo.AddAsync(favorite);
            }
            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
