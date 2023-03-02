using productstockingv1.Interfaces;
using productstockingv1.models;

namespace productstockingv1.Repository;

public class WareRepository : Repository<Ware, string>
{
    public WareRepository(IProductContext context) : base(context)
    {
    }
}