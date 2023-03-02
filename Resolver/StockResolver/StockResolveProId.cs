using AutoMapper;
using productstockingv1.Interfaces;
using productstockingv1.models;
using productstockingv1.Models.Request;

namespace productstockingv1.Resolver;

public class StockResolveProId : IValueResolver<StockingCreateReq, Stocking, string>
{
    private readonly IUnitOfWork _context;
    public StockResolveProId(IUnitOfWork context) => _context = context;

    public string Resolve(StockingCreateReq source, Stocking destination, string destMember, ResolutionContext context)
    {
        var getProductId = _context.getRepository<Product, string>().GetAllQueryable()
            .FirstOrDefault(pro => pro.Code == source.ProductCode);
        return getProductId.Id;
    }
}

public class StockResolveNewProId : IValueResolver<StockCreateProdReq, Stocking, string>
{
    private readonly IUnitOfWork _context;
    public StockResolveNewProId(IUnitOfWork context) => _context = context;

    public string Resolve(StockCreateProdReq source, Stocking destination, string destMember, ResolutionContext context)
    {
        var getProductId = _context.getRepository<Product, string>().GetAllQueryable()
            .FirstOrDefault(pro => pro.Code == source.ProductCode);
        return getProductId.Id;
    }
}