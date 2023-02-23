using Microsoft.EntityFrameworkCore.Storage;
using productstockingv1.Interfaces;
using AutoMapper;

namespace productstockingv1.Repository;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly IProductContext _Context;
    
    public IProductContext dbContext { get; }

    private bool _disposed;
    
    private string _errorMessage = string.Empty;

    private IDbContextTransaction? objTran;

    private Dictionary<string, object> _repository = new();

    public UnitOfWork(IProductContext productContext) => _Context = productContext;

    public void BeginTransaction()
    {
        objTran = _Context.Database.BeginTransaction();
    }

    public void Commit()
    {
        objTran?.Commit();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void RollBack()
    {
        objTran?.Rollback();
        objTran.Dispose();
    }

    public void Save()
    {
        try
        {
            _Context.SaveChanges();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public IRepository<TE, TK> getRepository<TE, TK>() where TE : class, IKey<TK>
    {
        var type = typeof(TE).Name;
        if (!_repository.ContainsKey(type))
        {
            try
            {
                var repositoryType = typeof(Repository<TE, TK>);
                var repositoryInstance = Activator.CreateInstance(repositoryType, _Context);
                if (repositoryInstance == null) throw new Exception($"Failure in creating an instance of {type}");
                _repository.Add(type, repositoryInstance);
            }
            catch (Exception)
            {
                throw;
            }
        }
        return (IRepository<TE, TK>)_repository[type];
    }

    private void Dispose(bool disposing)
    {
        if(!_disposed)
            if(disposing) _Context.Dispose();
        _disposed = true;
    } 
}