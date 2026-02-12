using FluentValidation;
using TK_UR_BOOK.Application.UseCases.BookQuery;

namespace TK_UR_BOOK.Application.Validations.QueryValidator
{
    public class GetBookQueryValidator : AbstractValidator<GetBookQuery>
    {
        public GetBookQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page index must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage("Page size must be between 1 and 50.");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue)
                .WithMessage("Minimum price cannot be less than zero.");

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice!.Value)
                .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue)
                .WithMessage("Maximum price must be greater than or equal to minimum price.");
            var allowedSorts = new[] { "priceasc", "pricedesc", "titleasc", "titledesc" };
            RuleFor(x => x.Sort)
                .Must(s => string.IsNullOrEmpty(s) || allowedSorts.Contains(s.ToLower()))
                .WithMessage("Selected sort option is not supported.");
        }
    }
}