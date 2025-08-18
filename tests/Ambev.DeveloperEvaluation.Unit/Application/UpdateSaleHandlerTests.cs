using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentValidation;
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
            Assert.Equal(command.Items.Sum(x => x.Quantity * x.ProductPrice), sale.TotalAmount);
        }

        [Fact(DisplayName = "Given non-existing sale When updating sale Then throws ValidationException")]
        public async Task Handle_NonExistingSale_ThrowsValidationException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = UpdateSaleHandlerTestData.GenerateValidCommand();

            _saleRepository.GetByIdAsync(command.Id, cancellationToken).Returns((Sale)null);

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Sale not found", exception.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
