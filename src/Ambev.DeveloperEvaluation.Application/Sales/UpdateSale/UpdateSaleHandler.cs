using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Validation.Helper;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale is null)
                throw new BusinessException("Sale not found");

            sale.Cancelled = command.Cancelled;

            sale.ClearItems();

            foreach (var itemCommand in command.Items)
            {
                var externalProduct = _mapper.Map<ProductExternalIdentity>(itemCommand);

                sale.AddItem(externalProduct, itemCommand.Quantity);
            }

            var saleValidationResult = sale.Validate();

            if (!saleValidationResult.IsValid)
                throw new ValidationException(ValidationHelper.GetValidationFailures(saleValidationResult));

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // TODO: Event Publishing

            return _mapper.Map<UpdateSaleResult>(sale);
        }
    }
}
