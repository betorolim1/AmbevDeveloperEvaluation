using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using AutoMapper;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetSalesHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        private readonly GetSalesHandler _handler;

        public GetSalesHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetSaleProfile>();
            });

            _mapper = configuration.CreateMapper();

            _handler = new GetSalesHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given invalid PageNumber Then throws ValidationException")]
        public async Task Handle_InvalidPageNumber_ThrowsValidationException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 0,
                PageSize = 10
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Invalid PageNumber", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact(DisplayName = "Given invalid PageSize Then throws ValidationException")]
        public async Task Handle_InvalidPageSize_ThrowsValidationException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 0
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, cancellationToken));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Invalid PageSize", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact(DisplayName = "Given valid command data When getting sales Then returns paged result")]
        public async Task Handle_ValidRequest_ReturnsPagedResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 10
            };

            var saleList = new List<Sale>{
                SaleTestData.GenerateValidSale(),
                SaleTestData.GenerateValidSale()
            };

            var saleQueryable = saleList.AsQueryable();

            var asyncEnumerable = new TestAsyncEnumerable<Sale>(saleQueryable);

            _saleRepository.Query().Returns(asyncEnumerable);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.PageNumber, result.CurrentPage);
            Assert.Equal(command.PageSize, result.PageSize);
            Assert.Equal(saleList.Count, result.TotalItems);
        }

        [Fact(DisplayName = "Given valid sales found When must filter by customer name Then returns filtered results")]
        public async Task Handle_ValidSalesFound_MustFilterByCustomerName_ReturnsFilteredResults()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                CustomerName = "Customer Test"
            };

            var saleForFilter = SaleTestData.GenerateValidSale();
            saleForFilter.Customer.CustomerName = "Customer Test";

            var saleList = new List<Sale>
            {
                SaleTestData.GenerateValidSale(),
                saleForFilter,
                SaleTestData.GenerateValidSale()
            };

            var saleQueryable = saleList.AsQueryable();

            var asyncEnumerable = new TestAsyncEnumerable<Sale>(saleQueryable);

            _saleRepository.Query().Returns(asyncEnumerable);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(saleForFilter.Customer.CustomerName, result.Items.First().CustomerName);
            Assert.Equal(saleForFilter.Id, result.Items.First().Id);
        }

        [Fact(DisplayName = "Given valid sales found When must filter by branch name Then returns filtered results")]
        public async Task Handle_ValidSalesFound_MustFilterByBranchName_ReturnsFilteredResults()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                BranchName = "Branch Test"
            };

            var saleForFilter = SaleTestData.GenerateValidSale();

            saleForFilter.Branch.BranchName = "Branch Test";

            var saleList = new List<Sale>
            {
                SaleTestData.GenerateValidSale(),
                saleForFilter,
                SaleTestData.GenerateValidSale()
            };

            var saleQueryable = saleList.AsQueryable();

            var asyncEnumerable = new TestAsyncEnumerable<Sale>(saleQueryable);

            _saleRepository.Query().Returns(asyncEnumerable);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(saleForFilter.Branch.BranchName, result.Items.First().BranchName);
            Assert.Equal(saleForFilter.Id, result.Items.First().Id);
        }

        [Fact(DisplayName = "Given valid sales found When must filter by start date Then returns filtered results")]
        public async Task Handle_ValidSalesFound_MustFilterByStartDate_ReturnsFilteredResults()
        {
            // Arrange
            var filterDate = DateTime.UtcNow.AddDays(5);

            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                StartDate = filterDate
            };

            var saleForFilter = SaleTestData.GenerateValidSale();

            saleForFilter.Date = filterDate;

            var saleList = new List<Sale>
            {
                SaleTestData.GenerateValidSale(),
                saleForFilter,
                SaleTestData.GenerateValidSale()
            };

            var saleQueryable = saleList.AsQueryable();

            var asyncEnumerable = new TestAsyncEnumerable<Sale>(saleQueryable);

            _saleRepository.Query().Returns(asyncEnumerable);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(saleForFilter.Date, result.Items.First().Date);
            Assert.Equal(saleForFilter.Id, result.Items.First().Id);
        }

        [Fact(DisplayName = "Given valid sales found When must filter by end date Then returns filtered results")]
        public async Task Handle_ValidSalesFound_MustFilterByEndDate_ReturnsFilteredResults()
        {
            // Arrange
            var filterDate = DateTime.UtcNow.AddDays(-5);

            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                EndDate = filterDate
            };

            var saleForFilter = SaleTestData.GenerateValidSale();

            saleForFilter.Date = filterDate;

            var saleList = new List<Sale>
            {
                SaleTestData.GenerateValidSale(),
                saleForFilter,
                SaleTestData.GenerateValidSale()
            };

            var saleQueryable = saleList.AsQueryable();

            var asyncEnumerable = new TestAsyncEnumerable<Sale>(saleQueryable);

            _saleRepository.Query().Returns(asyncEnumerable);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(saleForFilter.Date, result.Items.First().Date);
            Assert.Equal(saleForFilter.Id, result.Items.First().Id);
        }

        [Fact(DisplayName = "Given valid sales found When must filter by cancelled flag Then returns filtered results")]
        public async Task Handle_ValidSalesFound_MustFilterByCancelledFlag_ReturnsFilteredResults()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                Cancelled = true
            };

            var saleForFilter = SaleTestData.GenerateValidSale();

            saleForFilter.Cancelled = true;

            var saleList = new List<Sale>
            {
                SaleTestData.GenerateValidSale(),
                saleForFilter,
                SaleTestData.GenerateValidSale()
            };

            var saleQueryable = saleList.AsQueryable();

            var asyncEnumerable = new TestAsyncEnumerable<Sale>(saleQueryable);

            _saleRepository.Query().Returns(asyncEnumerable);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(saleForFilter.Cancelled, result.Items.First().Cancelled);
            Assert.Equal(saleForFilter.Id, result.Items.First().Id);
        }

        [Fact(DisplayName = "Given valid sales found When must order by date Then returns ordered results")]
        public async Task Handle_ValidSalesFound_MustOrderByDate_ReturnsOrderedResults()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                OrderBy = "Date",
                Descending = true
            };

            var saleForFilter1 = SaleTestData.GenerateValidSale();
            saleForFilter1.Date = DateTime.UtcNow.AddDays(5);

            var saleForFilter2 = SaleTestData.GenerateValidSale();
            saleForFilter2.Date = DateTime.UtcNow.AddDays(10);

            var saleList = new List<Sale>
            {
                saleForFilter1,
                saleForFilter2
            };

            var saleQueryable = saleList.AsQueryable();

            var asyncEnumerable = new TestAsyncEnumerable<Sale>(saleQueryable);

            _saleRepository.Query().Returns(asyncEnumerable);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(saleForFilter2.Id, result.Items.First().Id);
        }

        [Fact(DisplayName = "Given valid sales found When must order by sale number Then returns ordered results")]
        public async Task Handle_ValidSalesFound_MustOrderBySaleNumber_ReturnsOrderedResults()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                OrderBy = "salenumber",
                Descending = true
            };

            var saleForFilter1 = SaleTestData.GenerateValidSale();
            saleForFilter1.SaleNumber = "234";

            var saleForFilter2 = SaleTestData.GenerateValidSale();
            saleForFilter2.SaleNumber = "123";

            var saleList = new List<Sale>
            {
                saleForFilter1,
                saleForFilter2
            };

            var saleQueryable = saleList.AsQueryable();

            var asyncEnumerable = new TestAsyncEnumerable<Sale>(saleQueryable);

            _saleRepository.Query().Returns(asyncEnumerable);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(saleForFilter1.Id, result.Items.First().Id);
        }

        [Fact(DisplayName = "Given valid sales found When must order by branch name Then returns ordered results")]
        public async Task Handle_ValidSalesFound_MustOrderByBranchName_ReturnsOrderedResults()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var command = new GetSalesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                OrderBy = "branchname",
                Descending = true
            };

            var saleForFilter1 = SaleTestData.GenerateValidSale();
            saleForFilter1.Branch.BranchName = "Branch B";

            var saleForFilter2 = SaleTestData.GenerateValidSale();
            saleForFilter2.Branch.BranchName = "Branch A";

            var saleList = new List<Sale>
            {
                saleForFilter1,
                saleForFilter2
            };

            var saleQueryable = saleList.AsQueryable();

            var asyncEnumerable = new TestAsyncEnumerable<Sale>(saleQueryable);

            _saleRepository.Query().Returns(asyncEnumerable);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(saleForFilter1.Id, result.Items.First().Id);
        }
    }
}
