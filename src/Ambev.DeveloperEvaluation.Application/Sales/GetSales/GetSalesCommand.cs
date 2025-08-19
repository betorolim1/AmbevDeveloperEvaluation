using Ambev.DeveloperEvaluation.Application.PageResult;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesCommand : IRequest<PagedResult<GetSalesResult>>
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
