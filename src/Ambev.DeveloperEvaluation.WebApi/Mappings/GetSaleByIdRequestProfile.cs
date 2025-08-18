using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleById;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class GetSaleByIdRequestProfile : Profile
    {
        public GetSaleByIdRequestProfile()
        {
            CreateMap<GetSaleByIdRequest, GetSaleByIdCommand>();

            CreateMap<GetSaleByIdResult, GetSaleByIdResponse>();
            CreateMap<GetSaleByIdItemResult, GetSaleByIdItemResponse>();
        }
    }
}
