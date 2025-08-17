namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale

{
    public class CreateSaleRequest
    {
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public List<CreateSaleItemRequest> Items { get; set; } = new();

        public bool Cancelled { get; set; } = false;
    }
}
