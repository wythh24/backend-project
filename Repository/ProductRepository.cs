using productstockingv1.Interfaces;
using productstockingv1.models;

namespace productstockingv1.Repository;

public class ProductRepository : Repository<Product, string>
{
    public ProductRepository(IProductContext context) : base(context)
    {
    }
}

public class StockRepository : Repository<Stocking, string>
{
    public StockRepository(IProductContext context) : base(context)
    {
        
    }
}
