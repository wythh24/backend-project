using AutoMapper;
using productstockingv1.models;
using productstockingv1.Models.Request;
using productstockingv1.models.StockRes;
using productstockingv1.Resolver;

namespace productstockingv1.EntityMapping;

public class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<Stocking, StockResponse>();

        CreateMap<StockingCreateReq, Stocking>()
            .ForMember(e => e.Id, option =>
                option.MapFrom(e => Guid.NewGuid().ToString()))
            .ForMember(e => e.ProductId, option => { option.MapFrom<StockResolveProId>(); }
            )
            .ForMember(e => e.WareId, option =>
                option.MapFrom<StockResolveWareId>()
            )
            .ForMember(e => e.DocumentDate, option => { option.MapFrom(e => DateTime.Now); });
        //create stock with new product
        CreateMap<StockCreateProdReq, Stocking>()
            .ForMember(e => e.Id, option =>
                option.MapFrom(e => Guid.NewGuid().ToString()))
            .ForMember(e => e.ProductId, option => { option.MapFrom<StockResolveNewProId>(); }
            )
            .ForMember(e => e.WareId, option =>
                option.MapFrom<StockResolveNewWareId>()
            )
            .ForMember(e => e.DocumentDate, option => { option.MapFrom(e => DateTime.Now); });
    }
}