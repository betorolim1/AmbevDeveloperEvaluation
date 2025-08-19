using Ambev.DeveloperEvaluation.Application.PageResult;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Unit.Helpers;
using Ambev.DeveloperEvaluation.Unit.WebApi.TestData;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Mappings;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi
{
    public class SalesControllerTests
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly SalesController _controller;

        public SalesControllerTests()
        {
            _mediator = Substitute.For<IMediator>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetSalesRequestProfile>();
                cfg.AddProfile<CreateSaleRequestProfile>();
                cfg.AddProfile<UpdateSaleRequestProfile>();
                cfg.AddProfile<GetSaleByIdRequestProfile>();
            });

            _mapper = config.CreateMapper();
            _controller = new SalesController(_mediator, _mapper);
        }

        // CreateSale

        [Fact(DisplayName = "CreateSale - Given invalid request Then returns BadRequest")]
        public async Task CreateSale_Given_InvalidRequest_Then_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateSaleRequest();

            // Act
            var result = await _controller.CreateSale(request, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            _mediator.VerifyNoOtherCalls(0);
        }

        [Fact(DisplayName = "CreateSale - Given valid request Then returns Created")]
        public async Task CreateSale_Given_ValidRequest_Then_ReturnsCreated()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var request = CreateSaleRequestTestData.GenerateValidCreateSaleRequest();

            _mediator.Send(Arg.Is<CreateSaleCommand>(x =>
                x.SaleNumber == request.SaleNumber &&
                x.CustomerId == request.CustomerId
            ), cancellationToken)
                .Returns(Task.FromResult(new CreateSaleResult { Id = Guid.NewGuid() }));

            // Act
            var result = await _controller.CreateSale(request, cancellationToken);

            // Assert
            var created = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, created.StatusCode);

            await _mediator.Received(1).Send(
                Arg.Is<CreateSaleCommand>(x =>
                    x.SaleNumber == request.SaleNumber &&
                    x.CustomerId == request.CustomerId),
                Arg.Any<CancellationToken>()
            );

            _mediator.VerifyNoOtherCalls(1);
        }

        // UpdateSale

        [Fact(DisplayName = "UpdateSale - Given Guid.Empty Then returns BadRequest")]
        public async Task UpdateSale_Given_EmptyGuid_Then_ReturnsBadRequest()
        {
            var request = new UpdateSaleRequest();

            var result = await _controller.UpdateSale(Guid.Empty, request, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);

            _mediator.VerifyNoOtherCalls(0);
        }

        [Fact(DisplayName = "UpdateSale - Given invalid request Then returns BadRequest")]
        public async Task UpdateSale_Given_InvalidRequest_Then_ReturnsBadRequest()
        {
            var request = new UpdateSaleRequest
            {
                Cancelled = true,
            };

            var result = await _controller.UpdateSale(Guid.NewGuid(), request, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
            _mediator.VerifyNoOtherCalls(0);
        }


        [Fact(DisplayName = "UpdateSale - Given valid request Then returns Ok")]
        public async Task UpdateSale_Given_ValidRequest_Then_ReturnsOk()
        {
            var cancellationToken = CancellationToken.None;
            var request = UpdateSaleRequestTestData.GenerateValidUpdateSaleRequest();

            _mediator.Send(Arg.Is<UpdateSaleCommand>(x =>
                x.Cancelled == request.Cancelled &&
                x.Items.Count == request.Items.Count
            ), cancellationToken)
                .Returns(Task.FromResult(new UpdateSaleResult { Id = Guid.NewGuid() }));

            var result = await _controller.UpdateSale(Guid.NewGuid(), request, cancellationToken);

            Assert.IsType<OkObjectResult>(result);

            await _mediator.Received(1).Send(
                Arg.Is<UpdateSaleCommand>(x =>
                    x.Cancelled == request.Cancelled &&
                    x.Items.Count == request.Items.Count
                ),
               cancellationToken);

            _mediator.VerifyNoOtherCalls(1);
        }

        // GetSales

        [Fact(DisplayName = "GetSales - Given request Then returns Ok with data")]
        public async Task GetSales_Given_ValidRequest_Then_ReturnsOk()
        {
            var cancellationToken = CancellationToken.None;
            var request = new GetSalesRequest { PageNumber = 1, PageSize = 5 };

            _mediator.Send(Arg.Is<GetSalesCommand>(x =>
                x.Cancelled == request.Cancelled &&
                x.CustomerName == request.CustomerName &&
                x.BranchName == request.BranchName &&
                x.StartDate == request.StartDate &&
                x.EndDate == request.EndDate &&
                x.PageNumber == request.PageNumber &&
                x.PageSize == request.PageSize &&
                x.OrderBy == request.OrderBy &&
                x.Descending == request.Descending
            ), cancellationToken)
                .Returns(new PagedResult<GetSalesResult>
                {
                    Items = new List<GetSalesResult>(),
                    CurrentPage = 1,
                    PageSize = 5,
                    TotalItems = 0
                });

            var result = await _controller.GetSales(request, cancellationToken);

            Assert.IsType<OkObjectResult>(result);

            await _mediator.Received(1).Send(
                Arg.Is<GetSalesCommand>(x =>
                    x.Cancelled == request.Cancelled &&
                    x.CustomerName == request.CustomerName &&
                    x.BranchName == request.BranchName &&
                    x.StartDate == request.StartDate &&
                    x.EndDate == request.EndDate &&
                    x.PageNumber == request.PageNumber &&
                    x.PageSize == request.PageSize &&
                    x.OrderBy == request.OrderBy &&
                    x.Descending == request.Descending
                ),
                cancellationToken);

            _mediator.VerifyNoOtherCalls(1);
        }

        // GetSaleById

        [Fact(DisplayName = "GetSaleById - Given id Then returns Ok with data")]
        public async Task GetSaleById_Given_ValidId_Then_ReturnsOk()
        {
            var cancellationToken = CancellationToken.None;
            var id = Guid.NewGuid();

            _mediator.Send(Arg.Is<GetSaleByIdCommand>(x =>
                x.Id == id
            ), cancellationToken)
                .Returns(Task.FromResult(new GetSaleByIdResult { Id = id }));

            var result = await _controller.GetSaleById(id, CancellationToken.None);

            Assert.IsType<OkObjectResult>(result);

            await _mediator.Received(1).Send(
                Arg.Is<GetSaleByIdCommand>(x => x.Id == id),
                cancellationToken);

            _mediator.VerifyNoOtherCalls(1);
        }

        // DeleteSale

        [Fact(DisplayName = "DeleteSale - Given id Then returns Ok and calls mediator")]
        public async Task DeleteSale_Given_ValidId_Then_ReturnsOk()
        {
            var cancellationToken = CancellationToken.None;
            var id = Guid.NewGuid();

            var result = await _controller.DeleteSale(id, cancellationToken);

            Assert.IsType<OkObjectResult>(result);

            await _mediator.Received(1).Send(
                Arg.Is<DeleteSaleCommand>(x => x.Id == id),
                cancellationToken);

            _mediator.VerifyNoOtherCalls(1);
        }
    }
}
