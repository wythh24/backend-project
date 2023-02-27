using productstockingv1.Interfaces;
using productstockingv1.models;

namespace productstockingv1.Repository;

public class StockingRepository : Repository<Stocking, string>
{
    public StockingRepository(IProductContext context) : base(context)
    {
    }
}