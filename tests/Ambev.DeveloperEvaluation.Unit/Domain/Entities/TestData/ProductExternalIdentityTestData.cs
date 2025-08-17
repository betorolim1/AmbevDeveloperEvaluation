using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class ProductExternalIdentityTestData
    {
        private static readonly Faker<ProductExternalIdentity> ProductExternalIdentityFaker = new Faker<ProductExternalIdentity>()
            .RuleFor(p => p.ProductId, f => f.Random.Guid())
            .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
            .RuleFor(p => p.ProductPrice, f => f.Random.Decimal(10, 1000));

        public static ProductExternalIdentity GenerateValidProductExternalIdentity()
        {
            return ProductExternalIdentityFaker.Generate();
        }
    }
}
