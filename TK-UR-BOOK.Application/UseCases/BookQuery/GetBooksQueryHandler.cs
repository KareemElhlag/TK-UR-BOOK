using Microsoft.EntityFrameworkCore;
using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.Validations.QueryValidator;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Application.UseCases.BookQuery
{
    public class GetBooksQueryHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GetBookQueryValidator _validationRules;

        public GetBooksQueryHandler(IUnitOfWork unitOfWork, GetBookQueryValidator validationRules)
        {
            _unitOfWork = unitOfWork;
            _validationRules = validationRules;
        }
        private IQueryable<BookDetailesDto> GetAllBooks()
        {
            return _unitOfWork.Repository<Book>().GetQueryable(predicate: b => !b.IsDeleted,
                incloud: q => q.Include(b => b.Category)).Select(b => new BookDetailesDto
                {

                    Id = b.Id.Value,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price.Amount,
                    Currency = b.Price.Currency.ToString(),
                    Stock = b.StockQuantity,
                    CategoryName = b.Category.Name


                });
        }


        public async Task<Result<List<BookDetailesDto>>> Handle(GetBookQuery request)
        {
            var validationResult = await _validationRules.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure<List<BookDetailesDto>>(validationResult.Errors.First().ErrorMessage);
            }

            var query = GetAllBooks();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(b => b.Title.Contains(request.Search) ||
                                         b.Author.Contains(request.Search) ||
                                         b.CategoryName!.Contains(request.Search));
            }

            if (request.Price.HasValue)
            {
                query = query.Where(b => b.Price == request.Price.Value);
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(q => q.Price >= request.MinPrice.Value);
            }
            if (request.MaxPrice.HasValue)
            {
                query = query.Where(q => q.Price <= request.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(request.Sort))
            {
                query = request.Sort.ToLower() switch
                {
                    "priceasc" => query.OrderBy(p => p.Price),
                    "pricedesc" => query.OrderByDescending(p => p.Price),
                    _ => query.OrderByDescending(p => p.Title)
                };
            }

            var books = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return Result.Success(books);
        }


    }
}
