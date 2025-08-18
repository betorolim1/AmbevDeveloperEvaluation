using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleById
{
    public class GetSaleByIdCommand : IRequest<GetSaleByIdResult>
    {
        public Guid Id { get; set; }
    }
}
