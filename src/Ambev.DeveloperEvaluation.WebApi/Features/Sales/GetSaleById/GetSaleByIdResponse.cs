namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleById
{
    public class GetSaleByIdResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public bool Cancelled { get; set; }
        public decimal TotalAmount { get; set; }
        public List<GetSaleByIdItemResponse> Items { get; set; } = new();
    }
}
