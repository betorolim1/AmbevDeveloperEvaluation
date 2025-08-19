using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.Items).NotNull().Must(items => items.Count > 0);
            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.ProductId).NotEmpty();
                items.RuleFor(i => i.Quantity).GreaterThan(0).LessThanOrEqualTo(20);
                items.RuleFor(i => i.ProductPrice).GreaterThan(0);
            });
            // TODO: Reuse Items validation from CreateSaleCommandValidator
        }
    }
}
