using Ambev.DeveloperEvaluation.Application.PageResult;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(result)
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Sale ID cannot be empty.");

        var validator = new UpdateSaleRequestValidator();
        var valResult = await validator.ValidateAsync(request);

        if (!valResult.IsValid)
            return BadRequest(valResult.Errors);

        var command = _mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<UpdateSaleResponse>
        {
            Success = true,
            Message = "Sale updated successfully",
            Data = _mapper.Map<UpdateSaleResponse>(result)
        });
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<List<GetSalesResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSales([FromQuery] GetSalesRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetSalesCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        var response = _mapper.Map<List<GetSalesResponse>>(result.Items);

        return Ok(new ApiResponseWithData<PagedResult<GetSalesResponse>>
        {
            Success = true,
            Message = "Sales retrieved successfully",
            Data = new PagedResult<GetSalesResponse>
            {
                Items = response,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            }
        });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaleById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new GetSaleByIdCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);

        var response = _mapper.Map<GetSaleByIdResponse>(result);

        return Ok(new ApiResponseWithData<GetSaleByIdResponse>
        {
            Success = true,
            Message = "Sale retrieved successfully",
            Data = response
        });
    }
}
