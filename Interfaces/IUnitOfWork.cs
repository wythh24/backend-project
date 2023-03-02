namespace productstockingv1.Interfaces;

public interface IUnitOfWork
{
    void BeginTransaction();
    void Commit();
    void Dispose();
    void RollBack();
    void Save();

    IRepository<TE, TK> getRepository<TE, TK>() where TE : class, IKey<TK>;
}