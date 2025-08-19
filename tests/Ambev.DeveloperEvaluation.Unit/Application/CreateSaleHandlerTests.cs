using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using AutoMapper;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _eventPublisher = Substitute.For<IEventPublisher>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CreateSaleProfile>();
            });

            _mapper = configuration.CreateMapper();

            _handler = new CreateSaleHandler(_saleRepository, _mapper, _eventPublisher);
        }

        [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var dateUtcNow = DateTime.UtcNow;

            var cancellationToken = CancellationToken.None;

            var command = CreateSaleHandlerTestData.GenerateValidCommand();

            var productExternalIdentity1 = new ProductExternalIdentity
            {
                ProductId = command.Items[0].ProductId,
                ProductName = command.Items[0].ProductName,
                ProductPrice = command.Items[0].ProductPrice
            };

            var productExternalIdentity2 = new ProductExternalIdentity
            {
                ProductId = command.Items[1].ProductId,
                ProductName = command.Items[1].ProductName,
                ProductPrice = command.Items[1].ProductPrice
            };

            var productExternalIdentity3 = new ProductExternalIdentity
            {
                ProductId = command.Items[2].ProductId,
                ProductName = command.Items[2].ProductName,
                ProductPrice = command.Items[2].ProductPrice
            };

            var saleWithItems = new Sale
            {
                SaleNumber = command.SaleNumber,
                Customer =
                    new CustomerExternalIdentity
                    {
                        CustomerId = command.CustomerId,
                        CustomerName = command.CustomerName
                    },
                Branch =
                    new BranchExternalIdentity
                    {
                        BranchId = command.BranchId,
                        BranchName = command.BranchName
                    },
                Cancelled = command.Cancelled
            };
            saleWithItems.AddItem(productExternalIdentity1, command.Items[0].Quantity);
            saleWithItems.AddItem(productExternalIdentity2, command.Items[1].Quantity);
            saleWithItems.AddItem(productExternalIdentity3, command.Items[2].Quantity);

            _saleRepository.CreateAsync(Arg.Is<Sale>(x =>
                x.Cancelled == command.Cancelled &&
                x.Date >= dateUtcNow &&
                x.SaleNumber == command.SaleNumber &&
                x.Customer.CustomerId == command.CustomerId &&
                x.Customer.CustomerName == command.CustomerName &&
                x.Branch.BranchId == command.BranchId &&
                x.Branch.BranchName == command.BranchName &&
                x.Items.Count == command.Items.Count &&
                x.Items.First().Total == 20 &&
                x.Items.First().Discount == 0 &&
                x.Items.ElementAt(1).Total == 450 &&
                x.Items.ElementAt(1).Discount == 50 &&
                x.Items.ElementAt(2).Total == 12000 &&
                x.Items.ElementAt(2).Discount == 3000
            ), cancellationToken).Returns(saleWithItems);

            // Act
            var response = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(command.SaleNumber, response.SaleNumber);
            Assert.True(response.Date <= dateUtcNow);
            Assert.Equal(command.Cancelled, response.Cancelled);
            Assert.Equal(12470, response.TotalAmount);

            await _saleRepository.Received(1).CreateAsync(Arg.Is<Sale>(x =>
                x.Cancelled == command.Cancelled &&
                x.Date >= dateUtcNow &&
                x.SaleNumber == command.SaleNumber &&
                x.Customer.CustomerId == command.CustomerId &&
                x.Customer.CustomerName == command.CustomerName &&
                x.Branch.BranchId == command.BranchId &&
                x.Branch.BranchName == command.BranchName &&
                x.Items.Count == command.Items.Count &&
                x.Items.First().Total == 20 &&
                x.Items.First().Discount == 0 &&
                x.Items.ElementAt(1).Total == 450 &&
                x.Items.ElementAt(1).Discount == 50 &&
                x.Items.ElementAt(2).Total == 12000 &&
                x.Items.ElementAt(2).Discount == 3000
            ), cancellationToken);

            await _eventPublisher.Received(1).PublishAsync(Arg.Is<SaleCreatedEvent>(x =>
                x.SaleId == saleWithItems.Id),
             cancellationToken);

            _saleRepository.VerifyNoOtherCalls(1);
            _eventPublisher.VerifyNoOtherCalls(1);
        }

        [Fact(DisplayName = "Given invalid command data When creating sale Then throws validation exception")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = CreateSaleHandlerTestData.GenerateInvalidCommand();

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                    _handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Validation failed", exception.Message, StringComparison.OrdinalIgnoreCase);

            _saleRepository.VerifyNoOtherCalls(0);
            _eventPublisher.VerifyNoOtherCalls(0);
        }
    }
}
