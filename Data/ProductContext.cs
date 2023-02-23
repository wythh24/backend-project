using System.Reflection;
using Microsoft.EntityFrameworkCore;
using productstockingv1.Interfaces;
using productstockingv1.models;

namespace productstockingv1.Data;

public class ProductContext : DbContext, IProductContext
{
    public ProductContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Ware> Wares { get; set; }
    public DbSet<Stocking> Stockings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}