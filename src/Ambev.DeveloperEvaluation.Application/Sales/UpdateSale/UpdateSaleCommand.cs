using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        public Guid Id { get; set; }
        public bool Cancelled { get; set; }
        public List<UpdateSaleItemCommand> Items { get; set; } = new();
    }
}
