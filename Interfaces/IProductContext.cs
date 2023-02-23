using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using productstockingv1.models;

namespace productstockingv1.Interfaces;

public interface IProductContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    int SaveChanges();

    DatabaseFacade Database { get; }

    void Dispose();

    DbSet<Product> Products { get; set; }
}