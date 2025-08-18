using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentValidation;
using NSubstitute;
using System.Threading;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CreateSaleProfile>();
            });

            _mapper = configuration.CreateMapper();

            _handler = new CreateSaleHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            var command = CreateSaleHandlerTestData.GenerateValidCommand();

            var productExternalIdentity = new ProductExternalIdentity
            {
                ProductId = command.Items[0].ProductId,
                ProductName = command.Items[0].ProductName,
                ProductPrice = command.Items[0].ProductPrice
            };

            var saleWithItems = new Sale(saleNumber: command.SaleNumber,
                                date: command.Date,
                                customer:
                                new CustomerExternalIdentity
                                {
                                    CustomerId = command.CustomerId,
                                    CustomerName = command.CustomerName
                                },
                                branch:
                                new BranchExternalIdentity
                                {
                                    BranchId = command.BranchId,
                                    BranchName = command.BranchName
                                },
                                cancelled: command.Cancelled);
            saleWithItems.AddItem(productExternalIdentity, command.Items[0].Quantity);

            _saleRepository.CreateAsync(Arg.Is<Sale>(x =>
                x.Cancelled == command.Cancelled &&
                x.Date == command.Date &&
                x.SaleNumber == command.SaleNumber &&
                x.Customer.CustomerId == command.CustomerId &&
                x.Customer.CustomerName == command.CustomerName &&
                x.Branch.BranchId == command.BranchId &&
                x.Branch.BranchName == command.BranchName
            ), cancellationToken).Returns(saleWithItems);

            // Act
            var response = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(command.SaleNumber, response.SaleNumber);
            Assert.Equal(command.Date, response.Date);
            Assert.Equal(command.Cancelled, response.Cancelled);
            Assert.Equal(command.Items.Sum(x => x.Quantity * x.ProductPrice), response.TotalAmount);
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
        }
    }
}
