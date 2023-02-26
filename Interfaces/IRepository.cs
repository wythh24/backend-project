namespace productstockingv1.Interfaces;

public interface IRepository<TE, TK> where TE : class, IKey<TK>
{
    IProductContext Context { get; }

    void Create(TE entity);
    Task CreateAsync(TE entity);

    void CreateBatch(IEnumerable<TE> entity);
    Task CreateBatchAsync(IEnumerable<TE> entity);

    void Delete(TE entity);
    Task DeleteAsync(TE entity);

    void DeleteBatch(IEnumerable<TE> entity);
    Task DeleteBatchAsync(IEnumerable<TE> entity);

    TE? Get(TK id);
    TE? Getpro(TK productid);
    Task<TE?>? GetAsync(TK id);
    Task<TE?>? GetAsyncd(TK productid);
    IQueryable<TE> GetAllQueryable();

    void Update(TE entity);
    Task UpdateAsync(TE entity);

    void UpdateBatch(IEnumerable<TE> entity);
    Task UpdateBatchAsync(IEnumerable<TE> entity);
    
}