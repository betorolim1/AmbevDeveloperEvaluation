using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.SaleNumber).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.BranchId).NotEmpty();
            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.ProductId).NotEmpty();
                items.RuleFor(i => i.Quantity).GreaterThan(0).LessThanOrEqualTo(20);
                items.RuleFor(i => i.ProductPrice).GreaterThan(0);
            });
        }
    }
}
