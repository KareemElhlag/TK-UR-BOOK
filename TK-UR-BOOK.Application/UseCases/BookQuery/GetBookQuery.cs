using TK_UR_BOOK.Application.DTOs;

namespace TK_UR_BOOK.Application.UseCases.BookQuery
{
    public sealed record GetBookQuery(string? Search,
    decimal? Price,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? Sort,
    int PageIndex = 1,
    int PageSize = 10);
    
    
}
