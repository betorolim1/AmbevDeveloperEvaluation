using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetSaleByIdHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        private readonly GetSaleByIdHandler _handler;

        public GetSaleByIdHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetSaleByIdProfile>();
            });

            _mapper = configuration.CreateMapper();

            _handler = new GetSaleByIdHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given not found sale Then throws ValidationException")]
        public async Task Handle_GivenNotFoundSale_ThenThrowsValidationException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSaleByIdCommand { Id = Guid.NewGuid() };

            _saleRepository.GetByIdAsync(command.Id, cancellationToken).Returns((Sale?)null);

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Sale not found", exception.Message, StringComparison.OrdinalIgnoreCase);

            await _saleRepository.Received(1).GetByIdAsync(command.Id, cancellationToken);

            _saleRepository.VerifyNoOtherCalls(1);
        }

        [Fact(DisplayName = "Given found sale Then returns GetSaleByIdResult")]
        public async Task Handle_GivenFoundSale_ThenReturnsGetSaleByIdResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSaleByIdCommand { Id = Guid.NewGuid() };

            var sale = SaleTestData.GenerateValidSale();
            sale.AddItem(
                new DeveloperEvaluation.Domain.Entities.ExternalIdentities.ProductExternalIdentity
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product test",
                    ProductPrice = 100.0m
                },
                quantity: 2
            );

            _saleRepository.GetByIdAsync(command.Id, cancellationToken).Returns(sale);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sale.Id, result.Id);
            Assert.Equal(sale.SaleNumber, result.SaleNumber);
            Assert.Equal(sale.Date, result.Date);
            Assert.Equal(sale.Customer.CustomerName, result.CustomerName);
            Assert.Equal(sale.Branch.BranchName, result.BranchName);
            Assert.Equal(sale.Cancelled, result.Cancelled);
            Assert.Equal(sale.Items.Count, result.Items.Count);
            Assert.Equal(sale.Items.First().Quantity, result.Items.First().Quantity);
            Assert.Equal(sale.Items.First().Product.ProductPrice, result.Items.First().ProductPrice);
            Assert.Equal(sale.Items.First().Product.ProductName, result.Items.First().ProductName);
            Assert.Equal(sale.Items.First().Product.ProductId, result.Items.First().ProductId);
            Assert.Equal(sale.Items.First().Total, result.Items.First().Total);
            Assert.Equal(sale.Items.First().Discount, result.Items.First().Discount);

            await _saleRepository.Received(1).GetByIdAsync(command.Id, cancellationToken);

            _saleRepository.VerifyNoOtherCalls(1);
        }
    }
}
