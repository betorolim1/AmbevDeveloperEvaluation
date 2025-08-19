using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Ambev.DeveloperEvaluation.Domain.Logger.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Validation.Helper;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
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

            sale.Date = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            var saleValidationResult = sale.Validate();

            if (!saleValidationResult.IsValid)
                throw new ValidationException(ValidationHelper.GetValidationFailures(saleValidationResult));

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

            var saleCreatedEvent = new SaleCreatedEvent(SaleId: sale.Id);

            SaleEventLogger.LogSaleCreated(saleCreatedEvent);

            await _eventPublisher.PublishAsync(new SaleCreatedEvent(createdSale.Id), cancellationToken);

            var response = _mapper.Map<CreateSaleResult>(createdSale);
            return response;
        }
    }
}