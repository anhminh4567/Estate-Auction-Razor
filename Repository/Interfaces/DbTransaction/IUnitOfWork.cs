namespace Repository.Interfaces.DbTransaction
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryWrapper Repositories { get; }
        Task BeginTransaction();
        Task CommitAsync();
        Task RollBackAsync();
        Task SaveChangesAsync();
    }
}
