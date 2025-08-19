using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales
{
    public class GetSalesRequest
    {
        public string? CustomerName { get; set; }
        public string? BranchName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Cancelled { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public SalesOrderBy OrderBy { get; set; } = SalesOrderBy.Date;
        public bool Descending { get; set; } = true;
    }
}
