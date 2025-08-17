using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class CustomerExternalIdentityTestData
    {
        private static readonly Faker<CustomerExternalIdentity> CustomerExternalIdentityFaker = new Faker<CustomerExternalIdentity>()
            .RuleFor(p => p.CustomerId, f => f.Random.Guid())
            .RuleFor(p => p.CustomerName, f => f.Commerce.ProductName());

        public static CustomerExternalIdentity GenerateValidCustomerExternalIdentity()
        {
            return CustomerExternalIdentityFaker.Generate();
        }
    }
}
