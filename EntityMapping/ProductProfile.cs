using AutoMapper;
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
    }
}

public class WareProfile : Profile
{
    private readonly Random _getCode = new Random();

    public WareProfile()
    {
        CreateMap<Ware,WareResponse>();
    }
}