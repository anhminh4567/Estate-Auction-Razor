
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Database;
using Repository.Interfaces.DbTransaction;

namespace Repository.Implementation.DbTransaction
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuctionRealEstateDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public IRepositoryWrapper Repositories { get; private set; }
        public UnitOfWork(AuctionRealEstateDbContext context, IRepositoryWrapper repository)
        {
            _context = context;
            Repositories = repository;

        }
        public Task BeginTransaction()
        {
            _currentTransaction = _context.Database.BeginTransaction();
            return Task.CompletedTask;
        }

        public Task CommitAsync()
        {
            if (_currentTransaction is null)
                throw new InvalidOperationException(" a transaction has not been initialized");
            return _currentTransaction.CommitAsync();
        }


        public Task RollBackAsync()
        {
            if (_currentTransaction is null)
                throw new InvalidOperationException(" a transaction has not been initialized");
            return _currentTransaction.RollbackAsync();
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            //_currentTransaction = null;
            _context.Dispose();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
