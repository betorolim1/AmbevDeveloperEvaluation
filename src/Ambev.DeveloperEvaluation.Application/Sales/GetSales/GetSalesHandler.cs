using Ambev.DeveloperEvaluation.Application.PageResult;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesHandler : IRequestHandler<GetSalesCommand, PagedResult<GetSalesResult>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetSalesResult>> Handle(GetSalesCommand request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0)
                throw new ValidationException("Invalid PageNumber");

            if (request.PageSize <= 0 )
                throw new ValidationException("Invalid PageSize");

            var query = _saleRepository.Query();

            var totalItems = await query.CountAsync(cancellationToken);

            query = GetFilters(query, request);

            query = GetOrderBy(query, request);

            query = GetPages(query, request);

            var saleList = await query.AsNoTracking().ToListAsync(cancellationToken);

            return new PagedResult<GetSalesResult>
            {
                Items = _mapper.Map<List<GetSalesResult>>(saleList),
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems
            };
        }

        private IQueryable<Sale> GetFilters(IQueryable<Sale> query, GetSalesCommand request)
        {
            if (!string.IsNullOrEmpty(request.CustomerName))
                query = query.Where(s => s.Customer.CustomerName.Contains(request.CustomerName));
            if (!string.IsNullOrEmpty(request.BranchName))
                query = query.Where(s => s.Branch.BranchName.Contains(request.BranchName));
            if (request.StartDate.HasValue)
                query = query.Where(s => s.Date >= request.StartDate.Value);
            if (request.EndDate.HasValue)
                query = query.Where(s => s.Date <= request.EndDate.Value);
            if (request.Cancelled.HasValue)
                query = query.Where(s => s.Cancelled == request.Cancelled.Value);
            return query;
        }

        private IQueryable<Sale> GetOrderBy(IQueryable<Sale> query, GetSalesCommand request)
        {
            return request.OrderBy?.ToLower() switch
            {
                "salenumber" => request.Descending ? query.OrderByDescending(s => s.SaleNumber) : query.OrderBy(s => s.SaleNumber),
                "branchname" => request.Descending ? query.OrderByDescending(s => s.Branch.BranchName) : query.OrderBy(s => s.Branch.BranchName),
                _ => request.Descending ? query.OrderByDescending(s => s.Date) : query.OrderBy(s => s.Date)
            };
        }

        private IQueryable<Sale> GetPages(IQueryable<Sale> query, GetSalesCommand request)
        {
            return query.Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize);
        }
    }
}
