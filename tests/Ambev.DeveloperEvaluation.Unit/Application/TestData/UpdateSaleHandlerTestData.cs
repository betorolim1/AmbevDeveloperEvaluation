using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class UpdateSaleHandlerTestData
    {
        private static readonly Faker<UpdateSaleCommand> updateSaleHandlerFaker = new Faker<UpdateSaleCommand>()
            .RuleFor(u => u.Id, f => f.Random.Guid())
            .RuleFor(u => u.Items, f => new List<UpdateSaleItemCommand>
            {
                new UpdateSaleItemCommand
                {
                    ProductId = f.Random.Guid(),
                    ProductName = f.Commerce.ProductName(),
                    ProductPrice = f.Finance.Amount(1, 100),
                    Quantity = f.Random.Int(1, 20)
                }
            })
            .RuleFor(u => u.Cancelled, f => f.Random.Bool());

        private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean13())
            .RuleFor(s => s.Date, f => f.Date.Past(1))
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
            .RuleFor(s => s.Cancelled, f => f.Random.Bool());

        public static UpdateSaleCommand GenerateValidCommand()
        {
            return updateSaleHandlerFaker.Generate();
        }

        public static Sale GenerateValidSale()
        {
            return saleFaker.Generate();
        }

        public static UpdateSaleCommand GenerateInvalidCommand()
        {
            var command = updateSaleHandlerFaker.Generate();
            command.Id = Guid.Empty;

            return command;
        }
    }
}
