using Microsoft.AspNetCore.Mvc;
using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.UseCases.BookQuery;
using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
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
    }
}
