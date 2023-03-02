using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using productstockingv1.models;
using productstockingv1.Models.Request;
using productstockingv1.models.StockRes;


namespace productstockingv1.EntityMapping;

public class ProductProfile : Profile
{
    private readonly Random _genCode = new Random();

    public ProductProfile()
    {
        CreateMap<ProductCreateReq, Product>()
            .ForMember(e => e.Id,
                option =>
                    option.MapFrom(e => Guid.NewGuid().ToString())
            );
        CreateMap<Product, ProductResponse>();
        CreateMap<Product, StockProductRes>();

    }
}

