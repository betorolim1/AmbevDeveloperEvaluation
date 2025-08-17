using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Validation.Helper;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = _mapper.Map<Sale>(command);

            foreach (var item in command.Items)
            {
                var product = _mapper.Map<ProductExternalIdentity>(item);

                sale.AddItem(product, item.Quantity);
            }

            var saleValidationResult = sale.Validate();

            if (!saleValidationResult.IsValid)
                throw new ValidationException(ValidationHelper.GetValidationFailures(saleValidationResult));

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

            // TODO: Event Publishing

            var response = _mapper.Map<CreateSaleResult>(createdSale);
            return response;
        }
    }
}