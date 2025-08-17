using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleItemTestData
    {
        private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
            .RuleFor(si => si.Id, f => f.Random.Guid())
            .RuleFor(si => si.SaleId, f => f.Random.Guid())
            .RuleFor(si => si.Product, f => 
                new ProductExternalIdentity
                {
                    ProductId = f.Random.Guid(),
                    ProductName = f.Commerce.ProductName(),
                    ProductPrice = f.Random.Decimal(10, 1000)
                })
            .RuleFor(si => si.Quantity, f => f.Random.Int(1, 10));

        public static SaleItem GenerateValidSaleItem()
        {
            return SaleItemFaker.Generate();
        }
    }
}
