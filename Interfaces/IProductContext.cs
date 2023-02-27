using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using productstockingv1.models;
using AutoMapper;
using productstockingv1.Models.Request;

namespace productstockingv1.Interfaces;

public interface IProductContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    int SaveChanges();

    DatabaseFacade Database { get; }

    void Dispose();

    DbSet<Product> Products { get; set; }
    DbSet<Ware> Wares { get; set; }
    DbSet<Stocking> Stockings { get; set; }
}