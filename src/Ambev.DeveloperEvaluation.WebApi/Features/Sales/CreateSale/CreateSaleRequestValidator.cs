﻿using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.SaleNumber).NotEmpty();

            RuleFor(x => x.Date).NotEmpty();

            RuleFor(x => x.CustomerId).NotEmpty();

            RuleFor(x => x.CustomerName).NotEmpty();

            RuleFor(x => x.BranchId).NotEmpty();

            RuleFor(x => x.BranchName).NotEmpty();

            RuleForEach(s => s.Items).SetValidator(new CreateSaleItemRequestValidator());
        }
    }
}
