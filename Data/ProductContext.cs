using Microsoft.EntityFrameworkCore;
using productstockingv1.models;

namespace productstockingv1.Data;

public class ProductContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public ProductContext(DbContextOptions options) : base(options)
    {
    }
}