using System.Collections;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Infrastructure.Persistence.DBContext;
using TK_UR_BOOK.Infrastructure.Repositories;

namespace TK_UR_BOOK.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Hashtable? _repositories;
        public UnitOfWork(AppDbContext appDb)
        {
            _context = appDb;
        }
        public async Task DisposeAsync()
        => await _context.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<TEntity>)_repositories[type]!;
        }

        public Task RollbackAsync()
        => _context.Database.RollbackTransactionAsync();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
    }
}
