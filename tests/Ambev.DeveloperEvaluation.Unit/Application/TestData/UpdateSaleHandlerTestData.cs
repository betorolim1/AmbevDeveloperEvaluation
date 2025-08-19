using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
                    ProductPrice = 10,
                    Quantity = 2
                },
                new UpdateSaleItemCommand
                {
                    ProductId = f.Random.Guid(),
                    ProductName = f.Commerce.ProductName(),
                    ProductPrice = 100,
                    Quantity = 5
                },
                new UpdateSaleItemCommand
                {
                    ProductId = f.Random.Guid(),
                    ProductName = f.Commerce.ProductName(),
                    ProductPrice = 1000,
                    Quantity = 15
                }
            })
            .RuleFor(u => u.Cancelled, f => f.Random.Bool());

        public static UpdateSaleCommand GenerateValidCommand()
        {
            return updateSaleHandlerFaker.Generate();
        }

        public static UpdateSaleCommand GenerateInvalidCommand()
        {
            var command = updateSaleHandlerFaker.Generate();
            command.Id = Guid.Empty;

            return command;
        }
    }
}
