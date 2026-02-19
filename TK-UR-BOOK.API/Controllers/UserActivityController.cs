using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TK_UR_BOOK.Application.UseCases.Purchasing;
using TK_UR_BOOK.Application.UseCases.RatingBook;
using TK_UR_BOOK.Application.UseCases.RatingBooks;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;

namespace TK_UR_BOOK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivityController : ControllerBase
    {
        private readonly GetUserAllPurchaseQureyHandler _getUserAllPurchaseQureyHandler;
        private readonly CreateRatingCommandHandle _createRetingCommandHandler;
        public UserActivityController(GetUserAllPurchaseQureyHandler getUserAllPurchaseQureyHandler,
            CreateRatingCommandHandle createRatingCommandHandle
            )
        {
            _getUserAllPurchaseQureyHandler = getUserAllPurchaseQureyHandler;
            _createRetingCommandHandler = createRatingCommandHandle;
        }
        [HttpGet]
        public async Task<IActionResult> GetUserActivity([FromQuery] GetUserAllPurchaseQurey qurey)
        {
            var result = await _getUserAllPurchaseQureyHandler.Handler(qurey);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Error);


            }
        }

        [HttpPost("CreateRating")]
        public async Task<IActionResult> CreateRating(CreateRetingCommand command)
        {
            var result = await _createRetingCommandHandler.Handler(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }
    }
}
