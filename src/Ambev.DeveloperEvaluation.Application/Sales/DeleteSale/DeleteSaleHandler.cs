using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;

        public DeleteSaleHandler(ISaleRepository saleRepository, IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(DeleteSaleCommand command, CancellationToken cancellationToken)        {
            if(command.Id == Guid.Empty)
                throw new BusinessException("Sale ID cannot be empty.");

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
                throw new BusinessException("Sale not found");

            await _saleRepository.DeleteAsync(sale, cancellationToken);

            await _eventPublisher.PublishAsync(new SaleDeleteEvent(sale.Id));
        }
    }
}
