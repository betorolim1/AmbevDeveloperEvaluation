using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new CustomerExternalIdentity
            {
                CustomerId = src.CustomerId,
                CustomerName = src.CustomerName
            }))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => new BranchExternalIdentity
            {
                BranchId = src.BranchId,
                BranchName = src.BranchName
            }))
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<CreateSaleItemCommand, ProductExternalIdentity>();

        CreateMap<Sale, CreateSaleResult>();
    }
}
