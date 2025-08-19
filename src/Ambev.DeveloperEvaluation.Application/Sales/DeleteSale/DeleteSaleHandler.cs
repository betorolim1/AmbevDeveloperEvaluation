using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand>
    {
        private readonly ISaleRepository _saleRepository;

        public DeleteSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(DeleteSaleCommand command, CancellationToken cancellationToken)        {
            if(command.Id == Guid.Empty)
                throw new ValidationException("Sale ID cannot be empty.");

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
                throw new ValidationException("Sale not found");

            await _saleRepository.DeleteAsync(sale, cancellationToken);

            // TODO: Publish Event
        }
    }
}
