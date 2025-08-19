using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
    public class UpdateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UpdateSaleProfile>();
            });

            _mapper = configuration.CreateMapper();

            _handler = new UpdateSaleHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = UpdateSaleHandlerTestData.GenerateValidCommand();

            var sale = SaleTestData.GenerateValidSale();
            sale.Cancelled = command.Cancelled;
            sale.Id = command.Id;

            _saleRepository.GetByIdAsync(command.Id, cancellationToken).Returns(sale);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Id);
            Assert.Equal(command.Cancelled, result.Cancelled);
            Assert.Equal(12470, sale.TotalAmount);

            await _saleRepository.Received(1).GetByIdAsync(command.Id, cancellationToken);

            await _saleRepository.Received(1).UpdateAsync(Arg.Is<Sale>(x => 
                x.Id == command.Id &&
                x.Cancelled == command.Cancelled &&
                x.Items.Count == command.Items.Count &&
                x.Items.All(i => command.Items.Any(c => c.ProductId == i.Product.ProductId && c.Quantity == i.Quantity)) &&
                x.Items.First().Total == 20 &&
                x.Items.First().Discount == 0 &&
                x.Items.ElementAt(1).Total == 450 &&
                x.Items.ElementAt(1).Discount == 50 &&
                x.Items.ElementAt(2).Total == 12000 &&
                x.Items.ElementAt(2).Discount == 3000
            ), cancellationToken);

            _saleRepository.VerifyNoOtherCalls(2);
        }

        [Fact(DisplayName = "Given non-existing sale When updating sale Then throws ValidationException")]
        public async Task Handle_NonExistingSale_ThrowsValidationException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = UpdateSaleHandlerTestData.GenerateValidCommand();

            _saleRepository.GetByIdAsync(command.Id, cancellationToken).Returns((Sale)null);

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Sale not found", exception.Message, StringComparison.OrdinalIgnoreCase);

            await _saleRepository.Received(1).GetByIdAsync(command.Id, cancellationToken);

            _saleRepository.VerifyNoOtherCalls(1);
        }
    }
}
