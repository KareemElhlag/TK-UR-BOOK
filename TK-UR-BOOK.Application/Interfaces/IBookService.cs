using System.Linq.Expressions;
using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.UseCases.BookQuery;
using TK_UR_BOOK.Application.UseCases.Purchasing;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Common;

namespace TK_UR_BOOK.Application.Interfaces
{
    public interface IBookService
    {
        Task<Result<Guid>> CreateBookAsync(CreateBookDto dto);
        Task<Result<BookDetailesDto>> GetBookByIdAsync(BookId id);
        Task<Result> UpdateBookAsync(UpdateBookDto bookDto);
        Task<Result> SoftDeleteBookAsync(BookId id);
        Task<Result<List<BookDetailesDto>>> GetAllBooks(GetBookQuery query);
        Task<Result> MarkBookAsOutOfStockAsync(BookId id);
        Task<Result> GetBookPurchasesAsync(GetBookAllPurchaseQurey command)

    }
}
