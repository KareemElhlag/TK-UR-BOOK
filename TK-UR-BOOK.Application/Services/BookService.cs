using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;
using TK_UR_BOOK.Domain.Enums;
using TK_UR_BOOK.Domain.ValueObjects;
using TK_UR_BOOK.Application.UseCases.BookQuery;
using TK_UR_BOOK.Application.UseCases.Purchasing;

namespace TK_UR_BOOK.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GetBooksQueryHandler _getBooksQueryHandler;
        private readonly GetBookAllPurchaseQureyHandler _getBookAllPurchaseQureyHandler;
        public BookService(IUnitOfWork unitOfWork, GetBooksQueryHandler getBooksQueryHandler, GetBookAllPurchaseQureyHandler getBookAllPurchaseQureyHandler)
        {
            _unitOfWork = unitOfWork;
            _getBooksQueryHandler = getBooksQueryHandler;
            _getBookAllPurchaseQureyHandler = getBookAllPurchaseQureyHandler;
        }
        public async Task<Result<Guid>> CreateBookAsync(CreateBookDto dto)
        {
            var book = new Book(
                new BookId(Guid.NewGuid()),
                title: dto.Title,
                author: dto.Author,
                isbn: dto.ISBN,
                price: new Money(dto.PriceAmount, dto.Currency),
                Qun: dto.Stock,
                categoryId: dto.CategoryId,
                description: dto.Description
                );
            await _unitOfWork.Repository<Book>().AddAsync(book);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(book.Id.Value);

        }

        public async Task<Result<List<BookDetailesDto>>> GetAllBooks(GetBookQuery query)
        {
            var books = await _getBooksQueryHandler.Handle(query);
            return books;
        }

        public async Task<Result<BookDetailesDto>> GetBookByIdAsync(BookId id)
        {
            var book = await _unitOfWork.Repository<Book>().GetByIdAsync(id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }
            var bookDetailsDto = new BookDetailesDto
            {
                Id = book.Id.Value,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price.Amount,
                Currency = book.Price.Currency.ToString(),
                Stock = book.StockQuantity
            };
            return Result<BookDetailesDto>.Success(bookDetailsDto);
        }

        public Task<Result> SoftDeleteBookAsync(BookId id)
        {
            var book = _unitOfWork.Repository<Book>().GetByIdAsync(id).Result;
            if (book == null)
            {
                return Task.FromResult(Result.Failure("Book not found"));
            }
            book.MarkAsDeleted();
            _unitOfWork.Repository<Book>().Update(book);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(Result.Success());
        }

        public async Task<Result> UpdateBookAsync(UpdateBookDto bookDto)
        {
            var book = await _unitOfWork.Repository<Book>().GetByIdAsync(bookDto.Id);
            if (book == null)
            {
                return Result.Failure("Book not found");
            }
            if (!Enum.TryParse<Currency>(bookDto.Currency, out var currency))
            {
                return Result.Failure("Invalid currency");
            }
            try
            {
                book.UpdateDetails(bookDto.Title, bookDto.Author, bookDto.Description);
                book.AdjustStock(bookDto.ChangeStock);
                book.UpdatePrice(new Money(bookDto.PriceAmount, currency));
                _unitOfWork.Repository<Book>().Update(book);
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            catch (ArgumentException ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> MarkBookAsOutOfStockAsync(BookId id)
        {
            var book = await _unitOfWork.Repository<Book>().GetByIdAsync(id);
            if (book == null)
            {
                return Result.Failure("Book not found");
            }
            book.MarkAsOutOfStock();
            _unitOfWork.Repository<Book>().Update(book);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> GetBookPurchasesAsync(GetBookAllPurchaseQurey command)
        {
            var purchases = await _getBookAllPurchaseQureyHandler.Handler(command);
            return Result.Success(purchases);
        }
    }

}
