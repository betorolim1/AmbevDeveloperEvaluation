namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequest
    {
        public bool Cancelled { get; set; }
        public List<UpdateSaleItemRequest> Items { get; set; } = new();
    }
}
