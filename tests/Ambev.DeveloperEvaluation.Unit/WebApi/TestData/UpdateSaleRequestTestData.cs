using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.TestData
{
    public static class UpdateSaleRequestTestData
    {
        private static readonly Faker<UpdateSaleRequest> updateSaleRequestFaker = new Faker<UpdateSaleRequest>()
            .RuleFor(p => p.Items, f => new List<UpdateSaleItemRequest>
            {
                new UpdateSaleItemRequest
                {
                    ProductId = f.Random.Guid(),
                    ProductName = f.Commerce.ProductName(),
                    Quantity = f.Random.Int(1, 10),
                    ProductPrice = f.Random.Decimal(10, 1000)
                }
            })
            .RuleFor(p => p.Cancelled, false);

        public static UpdateSaleRequest GenerateValidUpdateSaleRequest()
        {
            return updateSaleRequestFaker.Generate();
        }
    }
}
