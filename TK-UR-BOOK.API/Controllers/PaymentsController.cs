using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TK_UR_BOOK.Application.UseCases.Payment;
using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.Sp_Interface;

namespace TK_UR_BOOK.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentCheckoutCommandHandler _PaymantHandler;
        private readonly IUserContext _userContxt;
        public PaymentsController( IUserContext userContext , PaymentCheckoutCommandHandler handler)
        {
            _userContxt = userContext;
            _PaymantHandler = handler;
            
        }
        [HttpPost("checkout/{bookId}")]
        public async Task<IActionResult> CreateCheckout(Guid bookId, [FromQuery] int quantity)
        {
            var command = new PaymentCommand(
            new BookId(bookId),
            _userContxt.UserId,
            quantity);
            
            var result = await _PaymantHandler.Handler(command , default );

            if (result.IsFailure) {
                return BadRequest(result.Error);
            }

            return Ok(new {checkOutUrl = result.Value});
        }

    }
}
