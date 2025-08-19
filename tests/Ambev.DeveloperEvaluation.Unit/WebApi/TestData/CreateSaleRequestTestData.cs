using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.TestData
{
    public static class CreateSaleRequestTestData
    {
        private static readonly Faker<CreateSaleRequest> createSaleRequestFaker = new Faker<CreateSaleRequest>()
            .RuleFor(p => p.SaleNumber, f => f.Commerce.ProductName())
            .RuleFor(p => p.CustomerId, f => f.Random.Guid())
            .RuleFor(p => p.CustomerName, f => f.Commerce.ProductName())
            .RuleFor(p => p.BranchId, f => f.Random.Guid())
            .RuleFor(p => p.BranchName, f => f.Commerce.ProductName())
            .RuleFor(p => p.Items, f => new List<CreateSaleItemRequest>
            {
                new CreateSaleItemRequest
                {
                    ProductId = f.Random.Guid(),
                    ProductName = f.Commerce.ProductName(),
                    Quantity = f.Random.Int(1, 10),
                    ProductPrice = f.Random.Decimal(10, 1000)
                }
            })
            .RuleFor(p => p.Cancelled, false);

        public static CreateSaleRequest GenerateValidCreateSaleRequest()
        {
            return createSaleRequestFaker.Generate();
        }
    }
}
