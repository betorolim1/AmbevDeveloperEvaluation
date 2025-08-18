using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class SaleTestData
    {
        private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean13())
            .RuleFor(s => s.Date, f => DateTime.Now)
            .RuleFor(s => s.Customer, f => new CustomerExternalIdentity
            {
                CustomerId = f.Random.Guid(),
                CustomerName = f.Name.FullName()
            })
            .RuleFor(s => s.Branch, f => new BranchExternalIdentity
            {
                BranchId = f.Random.Guid(),
                BranchName = f.Company.CompanyName()
            })
            .RuleFor(s => s.Cancelled, f => false);

        public static Sale GenerateValidSale()
        {
            return saleFaker.Generate();
        }
    }
}
