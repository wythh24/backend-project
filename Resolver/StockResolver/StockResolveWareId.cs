using AutoMapper;
using productstockingv1.Interfaces;
using productstockingv1.models;
using productstockingv1.Models.Request;

namespace productstockingv1.Resolver;

public class StockResolveWareId : IValueResolver<StockingCreateReq, Stocking, string>
{
    private readonly IUnitOfWork _context;
    public StockResolveWareId(IUnitOfWork context) => _context = context;

    public string Resolve(StockingCreateReq source, Stocking destination, string destMember, ResolutionContext context)
    {
        var ware = _context.getRepository<Ware, string>().GetAllQueryable()
            .FirstOrDefault(wa => wa.Code == source.WareCode);
        return ware.Id;
    }
}