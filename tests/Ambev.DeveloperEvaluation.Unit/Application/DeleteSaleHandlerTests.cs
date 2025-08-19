using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;

        private readonly DeleteSaleHandler _handler;

        public DeleteSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();

            _handler = new DeleteSaleHandler(_saleRepository);
        }

        [Fact(DisplayName = "Given empty sale ID Then throws ValidationException")]
        public async Task Handle_GivenEmptySaleId_Then_ThrowsValidationException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new DeleteSaleCommand { Id = Guid.Empty };

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Sale ID cannot be empty.", exception.Message, StringComparison.OrdinalIgnoreCase);

            _saleRepository.VerifyNoOtherCalls(0);
        }

        [Fact(DisplayName = "Given not found sale Then throws ValidationException")]
        public async Task Handle_GivenNotFoundSale_Then_ThrowsValidationException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new DeleteSaleCommand { Id = Guid.NewGuid() };

            _saleRepository.GetByIdAsync(command.Id, cancellationToken).Returns((Sale?)null);

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Sale not found", exception.Message, StringComparison.OrdinalIgnoreCase);

            await _saleRepository.Received(1).GetByIdAsync(command.Id, cancellationToken);

            _saleRepository.VerifyNoOtherCalls(1);
        }

        [Fact(DisplayName = "Given found sale Then deletes sale successfully")]
        public async Task Handle_GivenFoundSale_Then_DeletesSaleSuccessfully()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new DeleteSaleCommand { Id = Guid.NewGuid() };

            var sale = SaleTestData.GenerateValidSale();

            _saleRepository.GetByIdAsync(command.Id, cancellationToken).Returns(sale);

            // Act
            await _handler.Handle(command, cancellationToken);

            // Assert
            await _saleRepository.Received(1).DeleteAsync(sale, cancellationToken);
            await _saleRepository.Received(1).GetByIdAsync(command.Id, cancellationToken);

            _saleRepository.VerifyNoOtherCalls(2);
        }
    }
}
