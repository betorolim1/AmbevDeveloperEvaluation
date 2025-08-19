using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker = new Faker<CreateSaleCommand>()
            .RuleFor(u => u.SaleNumber, f => f.Commerce.Ean13())
            .RuleFor(u => u.CustomerId, f => f.Random.Guid())
            .RuleFor(u => u.CustomerName, f => f.Name.FullName())
            .RuleFor(u => u.BranchId, f => f.Random.Guid())
            .RuleFor(u => u.BranchName, f => f.Company.CompanyName())
            .RuleFor(u => u.Items, f => new List<CreateSaleItemCommand>
            {
                new CreateSaleItemCommand
                {
                    ProductId = f.Random.Guid(),
                    ProductName = f.Commerce.ProductName(),
                    ProductPrice = 10,
                    Quantity = 2
                },
                new CreateSaleItemCommand
                {
                    ProductId = f.Random.Guid(),
                    ProductName = f.Commerce.ProductName(),
                    ProductPrice = 100,
                    Quantity = 5
                },
                new CreateSaleItemCommand
                {
                    ProductId = f.Random.Guid(),
                    ProductName = f.Commerce.ProductName(),
                    ProductPrice = 1000,
                    Quantity = 15
                }
            })
            .RuleFor(u => u.Cancelled, f => f.Random.Bool());

        public static CreateSaleCommand GenerateValidCommand()
        {
            return createSaleHandlerFaker.Generate();
        }

        public static CreateSaleCommand GenerateInvalidCommand()
        {
            var command = createSaleHandlerFaker.Generate();
            command.SaleNumber = string.Empty;

            return command;
        }
    }
}
