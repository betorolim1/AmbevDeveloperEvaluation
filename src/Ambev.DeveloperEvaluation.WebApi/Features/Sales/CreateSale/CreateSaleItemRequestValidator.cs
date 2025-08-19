using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
    {
        public CreateSaleItemRequestValidator()
        {
            RuleFor(i => i.ProductId).NotEmpty();
            RuleFor(i => i.Quantity).GreaterThan(0).LessThanOrEqualTo(20);
            RuleFor(i => i.ProductPrice).GreaterThan(0);
        }
    }
}
