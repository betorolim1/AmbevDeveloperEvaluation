using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class BranchExternalIdentityTestData
    {
        private static readonly Faker<BranchExternalIdentity> BranchExternalIdentityFaker = new Faker<BranchExternalIdentity>()
            .RuleFor(p => p.BranchId, f => f.Random.Guid())
            .RuleFor(p => p.BranchName, f => f.Commerce.ProductName());

        public static BranchExternalIdentity GenerateValidBranchExternalIdentity()
        {
            return BranchExternalIdentityFaker.Generate();
        }
    }
}
