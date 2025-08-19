using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(c => c.Items).NotNull().Must(items => items.Count > 0);
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).NotEmpty();
                item.RuleFor(i => i.ProductName).NotEmpty();
                item.RuleFor(i => i.ProductPrice).GreaterThan(0);
                item.RuleFor(i => i.Quantity)
                    .InclusiveBetween(1, 20).WithMessage("Quantity must be between 1 and 20.");
            });
        }
    }
}
