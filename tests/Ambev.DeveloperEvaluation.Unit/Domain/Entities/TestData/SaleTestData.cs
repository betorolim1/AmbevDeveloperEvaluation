using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleTestData
    {
        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean13())
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.Customer, f =>
                new CustomerExternalIdentity
                {
                    CustomerId = f.Random.Guid(),
                    CustomerName = f.Name.FullName()
                })
            .RuleFor(s => s.Branch, f =>
                new BranchExternalIdentity
                {
                    BranchId = f.Random.Guid(),
                    BranchName = f.Company.CompanyName()
                })
            .RuleFor(s => s.Cancelled, f => false);

        public static Sale GenerateValidSale()
        {
            return SaleFaker.Generate();
        }
    }
}
