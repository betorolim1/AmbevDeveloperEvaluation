using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(sale => sale.SaleNumber)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Sale number is required and cannot exceed 50 characters.");

            RuleFor(sale => sale.Date)
                .NotEmpty()
                .WithMessage("Sale date is required.");

            RuleFor(sale => sale.Customer)
                .NotNull()
                .WithMessage("Customer information is required.")
                .ChildRules(customer =>
                {
                    customer.RuleFor(c => c.CustomerId)
                        .NotEmpty().WithMessage("CustomerId is required.");

                    customer.RuleFor(c => c.CustomerName)
                        .NotEmpty().WithMessage("CustomerName is required.");
                });

            RuleFor(sale => sale.Branch)
                .NotNull()
                .WithMessage("Branch information is required.")
                .ChildRules(branch =>
                {
                    branch.RuleFor(b => b.BranchId)
                        .NotEmpty().WithMessage("BranchId is required.");

                    branch.RuleFor(b => b.BranchName)
                        .NotEmpty().WithMessage("BranchName is required.");
                });

            RuleForEach(sale => sale.Items)
                .NotNull()
                .ChildRules(item =>
                {
                    item.RuleFor(i => i.Product)
                        .NotNull().WithMessage("Product is required.")
                        .ChildRules(product =>
                        {
                            product.RuleFor(p => p.ProductId)
                                .NotEmpty().WithMessage("ProductId is required.");

                            product.RuleFor(p => p.ProductName)
                                .NotEmpty().WithMessage("ProductName is required.");

                            product.RuleFor(p => p.ProductPrice)
                                .GreaterThan(0).WithMessage("ProductPrice must be greater than zero.");
                        });

                    item.RuleFor(i => i.Quantity)
                        .InclusiveBetween(1, 20)
                        .WithMessage("Quantity must be between 1 and 20.");
                });
        }
    }
}