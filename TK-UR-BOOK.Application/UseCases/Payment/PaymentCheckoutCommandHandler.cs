using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using TK_UR_BOOK.Application.DTOs;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Application.Validations.PaymentValidator;
using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Entities;
using static System.Net.WebRequestMethods;

namespace TK_UR_BOOK.Application.UseCases.Payment
{
    public class PaymentCheckoutCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly StripeSettings _options;
        private readonly PaymantValidation _validationRules;
        private readonly IConfiguration _Config;
        public PaymentCheckoutCommandHandler(IUnitOfWork unitOfWork, IOptions<StripeSettings> options,
            PaymantValidation validationRules, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _options = options.Value;
            _validationRules = validationRules;
            _Config = configuration;
        }

        public async Task<Result<string>> Handler(PaymentCommand paymantCommand, CancellationToken cancellationToken)
        {
            var validiates = await _validationRules.ValidateAsync(paymantCommand);
            if (!validiates.IsValid)
            {
                return Result.Failure<string>(validiates.Errors.ToString()!);
            }

            var Book = await _unitOfWork.Repository<Book>().GetByIdAsync(paymantCommand.BookId);
            // set stripe secretKey
            StripeConfiguration.ApiKey = _options.secretKey;
            //setBase Url
            var baseUrl = _Config["AppSettings:BaseUrl"];
            // Build Session
            var options = new SessionCreateOptions
            {
                //payment method type set
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    //set Price Data  && Product Data
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)Book!.Price.Amount*100,
                            Currency = Book.Price.Currency.ToString().ToLower(),

                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = Book.Title,
                                Description = $"ISBN: {Book.ISBN}" ,
                            },

                        },
                        // set Quantity
                        Quantity = (long)paymantCommand.quantity,
                    },
                },
                Mode = "payment",
                Metadata = new Dictionary<string, string>
                {
                    { "BookId" , paymantCommand.BookId.Value.ToString() },
                    {"UserId" , paymantCommand.UserId.Value.ToString() },
                },
                SuccessUrl = $"{baseUrl}/payment/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{baseUrl}/payment/cancel",
            };

            try
            {
                var service = new SessionService();
                Session session = await service.CreateAsync(options, cancellationToken: cancellationToken);
                return Result.Success(session.Url);
            }
            catch (StripeException ex)
            {
                return Result.Failure<string>($"Stripe Error: {ex.Message}");
            }
        }



    }
}

