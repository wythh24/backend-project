using productstockingv1.Interfaces;

namespace productstockingv1.Repository;

public class Repository<TE, TK> : IRepository<TE, TK> where TE : class, IKey<TK>
     
{
    private readonly IProductContext _context;
    private IRepository<TE, TK> _repositoryImplementation;
    public Repository(IProductContext context) => _context = context;

    public virtual IProductContext Context => _context;

    public IQueryable<TE> GetAllQueryable() => _context.Set<TE>().AsQueryable();

    public TE? Get(TK id) => _context.Set<TE>().Find(id);
    public TE? Getpro(TK productid) => _context.Set<TE>().Find(productid);
    public async Task<TE?> GetAsyncd(TK productid) => await Task.FromResult(Getpro(productid));
    public async Task<TE?> GetAsync(TK id) => await Task.FromResult(Get(id));

    public void Create(TE entity)
    {
        try
        {
            _context.Set<TE>().Add(entity);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task CreateAsync(TE entity) => await Task.Run((() => Create(entity)));

    public void CreateBatch(IEnumerable<TE> entities)
    {
        try
        {
            _context.Set<TE>().AddRange(entities);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    

    public async Task CreateBatchAsync(IEnumerable<TE> entities) => await Task.Run((() => CreateBatch(entities)));

    // {
    //     await Task.Run((() => CreateBatch(entities)));
    // }
    public void Update(TE entity)
    {
        try
        {
            _context.Set<TE>().Update(entity);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateAsync(TE entity)
    {
        await Task.Run((() => Update(entity)));
    }

    public void UpdateBatch(IEnumerable<TE> entities)
    {
        try
        {
            _context.Set<TE>().UpdateRange(entities);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateBatchAsync(IEnumerable<TE> entities) => await Task.Run(() => UpdateBatch(entities));
    
    public virtual void Delete(TE entity)
    {
        try
        {
            _context.Set<TE>().Remove(entity);
            _context.SaveChanges();
        }
        catch (Exception) { throw; }
    }

    public async  Task DeleteAsync(TE entity)
    {
        await Task.Run(() => Delete(entity));
    }

    public virtual void DeleteBatch(IEnumerable<TE> entities)
    {
        try
        {
            _context.Set<TE>().RemoveRange(entities);
            _context.SaveChanges();
        }
        catch (Exception) { throw; }
    }
    public async  Task DeleteBatchAsync(IEnumerable<TE> entities)
    {
        await Task.Run(() => DeleteBatch(entities));
    }
}
