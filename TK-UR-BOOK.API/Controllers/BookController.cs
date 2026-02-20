using Microsoft.AspNetCore.Mvc;
using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.UseCases.BookQuery;
using TK_UR_BOOK.Application.UseCases.Favorites;
using TK_UR_BOOK.Application.UseCases.Purchasing;
using TK_UR_BOOK.Application.UseCases.RatingBook;
using TK_UR_BOOK.Application.UseCases.RatingBooks;
using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly CreateRatingCommandHandle _createRatingCommandHandle;
        private readonly GetBookRatingQureyHandler _getBookRatingQureyHandler;
        private readonly DeletRatingCommandHandler _deletRatingCommandHandler;
        private readonly ToggleFavoriteCommandHandler _toggleFavoriteCommandHandler;

        public BookController(IBookService bookService,
            CreateRatingCommandHandle createRatingCommandHandle,
            GetBookRatingQureyHandler getBookRatingQureyHandler,
            DeletRatingCommandHandler deletRatingCommandHandler,
            ToggleFavoriteCommandHandler toggleFavoriteCommandHandler
            )
        {
            _bookService = bookService;
            _createRatingCommandHandle = createRatingCommandHandle;
            _getBookRatingQureyHandler = getBookRatingQureyHandler;
            _deletRatingCommandHandler = deletRatingCommandHandler;
            _toggleFavoriteCommandHandler = toggleFavoriteCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookAsync(CreateBookDto dto)
        {
            var bookId = await _bookService.CreateBookAsync(dto);

            return Ok(new { Message = "Book Created Successfully", Id = bookId });
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] BookId id)
        {
            var bookDetails = await _bookService.GetBookByIdAsync(id);
            if (bookDetails.IsSuccess)
            {
                return Ok(bookDetails.Value);
            }
            return NotFound(bookDetails.Error);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBookAsync(UpdateBookDto dto)
        {
            var result = await _bookService.UpdateBookAsync(dto);
            if (result.IsSuccess)
            {
                return Ok(new { Message = "Book  Updated Successfully" });
            }
            return NotFound(result.Error);
        }
        [HttpDelete]
        public async Task<IActionResult> SoftDelete([FromQuery] BookId id)
        {
            var result = await _bookService.SoftDeleteBookAsync(id);
            if (result.IsSuccess)
            {
                return Ok(new { Message = "Book Deleted Successfully" });
            }
            return NotFound(result.Error);
        }
        [HttpGet("All")]
        ///
        /// <summary>
        /// this endpoint is used to get all books with filters , pagination , and sorting 
        /// <summary/>
        ///
        public async Task<IActionResult> GetBooksWithFiltratoinsAsync([FromQuery] GetBookQuery query)
        {
            var result = await _bookService.GetAllBooks(query);
            if (result.IsSuccess)
                return Ok(result);
            return NotFound(result.Error);
        }
        [HttpPut("MarkAsOutOfStock")]
        public async Task<IActionResult> MarkBookAsOutOfStockAsync([FromQuery] BookId id)
        {
            var result = await _bookService.MarkBookAsOutOfStockAsync(id);
            if (result.IsSuccess)
                return Ok(new { Message = "Book Marked as Out of Stock Successfully" });
            return NotFound(result.Error);
        }
        [HttpGet("Purchases")]
        public async Task<IActionResult> GetBookPurchasesAsync([FromQuery] GetBookAllPurchaseQurey query)
        {
            var result = await _bookService.GetBookPurchasesAsync(query);
            if (result.IsSuccess)
                return Ok(result);
            return NotFound(result.Error);

        }

        [HttpPost("CreateRating")]
        public async Task<IActionResult> CreateRating(CreateRetingCommand command)
        {
            var result = await _createRatingCommandHandle.Handler(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [HttpGet("GetAllRating")]
        public async Task<IActionResult> GetAllRatingAsync(GetBookRatingQurey query)
        {
            var result = await _getBookRatingQureyHandler.Handler(query);
            if (!result.IsSuccess)
                return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpDelete("DeletRating")]
        public async Task<IActionResult> DeletRating(DeletRatingCommand command)
        {
            var result = await _deletRatingCommandHandler.Handler(command);
            if (!result.IsSuccess)
                return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpPost("FavoriteToggle")]
        public async Task<IActionResult> FavToggleAsync(ToggleFavoriteCommand command, CancellationToken ct)
        {

            var result = await _toggleFavoriteCommandHandler.Handle(command, ct);
            if (!result.IsSuccess)
                return BadRequest(result.Error);
            return Ok(result);

        }
    }
}
