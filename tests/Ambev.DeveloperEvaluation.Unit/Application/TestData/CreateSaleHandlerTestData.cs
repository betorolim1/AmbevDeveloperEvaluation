using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker = new Faker<CreateSaleCommand>()
            .RuleFor(u => u.SaleNumber, f => f.Commerce.Ean13())
            .RuleFor(u => u.Date, f => f.Date.Past(1))
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
                    ProductPrice = f.Finance.Amount(1, 100),
                    Quantity = f.Random.Int(1, 20)
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
