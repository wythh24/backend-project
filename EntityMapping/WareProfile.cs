using AutoMapper;
using productstockingv1.models;
using productstockingv1.Models.Request;

namespace productstockingv1.EntityMapping;

public class WareProfile : Profile
{
    private readonly Random _getCode = new Random();

    public WareProfile()
    {
        CreateMap<Ware, WareResponse>();
        CreateMap<WareCreateReq, Ware>()
            .ForMember(e => e.Id,
                Option =>
                    Option.MapFrom(e => Guid.NewGuid().ToString())
            );

        CreateMap<WareUpdateReq, Ware>()
            .ForMember(e => e.Name, option =>
            {
                option.Condition(w => !string.IsNullOrEmpty(w.Id));
                option.MapFrom(w => w.name!.ToString());
            });
    }
}