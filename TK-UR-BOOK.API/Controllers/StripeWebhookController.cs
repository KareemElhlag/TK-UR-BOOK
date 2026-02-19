using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.V2.Core;
using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.UseCases.Payment;
using TK_UR_BOOK.Domain.Comman;
using Event = Stripe.Event;

namespace TK_UR_BOOK.Controllers
{
    [Route("api/webhooks")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly PaymentWebhookCommandHandler _webHookHandler;
        private readonly StripeSettings _stripeSettings;
        public StripeWebhookController(
            PaymentWebhookCommandHandler webHookHandler, IOptions<StripeSettings> stripeSettings)
        {
            _webHookHandler = webHookHandler;
            _stripeSettings = stripeSettings.Value;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> Handle()
        {
            try
            {
                // read Body requst
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                // get webHook SecretKay for comper with the local one 
                var signature = Request.Headers["Stripe-Signature"];

                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signature,
                    _stripeSettings.webhookKey
                    );

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    var command = new ConfirmPurchaseCommand(
                       new UserId(Guid.Parse(session!.Metadata["UserId"])),
                        new BookId(Guid.Parse(session!.Metadata["BookId"])),
                        session.PaymentIntentId,
                        (decimal)session.AmountTotal! / 100,
                        session.Currency
                        );

                    var result = await _webHookHandler.Handler(command);
                    if (result.IsFailure)
                        return StatusCode(500, result.Error);
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
