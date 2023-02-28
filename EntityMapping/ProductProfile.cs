using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using productstockingv1.models;
using productstockingv1.Models.Request;


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
    }
}

public class StockProfile : Profile
{
    private readonly Random _getCode = new Random();

    public StockProfile()
    {
        CreateMap<Stocking,StockResponse>();

        CreateMap<StockCreateRequest, Stocking>()
            .ForMember(e => e.Id,
                Option =>
                    Option.MapFrom(e => Guid.NewGuid().ToString()
                    )
            );
        CreateMap<Create, Stocking>()
            .ForMember(e=> e.Id,
                Option=>
                    Option.MapFrom(e=> Guid.NewGuid().ToString())
                    )
            .ForMember(e=> e.DocumentDate,
                Option=> 
                    Option.MapFrom(e=> DateTime.Now)
                    )
            ;
        

    }
}

