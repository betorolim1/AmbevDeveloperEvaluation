namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleById
{
    public class GetSaleByIdResult
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public bool Cancelled { get; set; }
        public decimal TotalAmount { get; set; }
        public List<GetSaleByIdItemResult> Items { get; set; } = new();
    }
}
