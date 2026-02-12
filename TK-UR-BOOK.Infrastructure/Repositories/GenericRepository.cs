using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TK_UR_BOOK.Application.Interfaces;
using TK_UR_BOOK.Infrastructure.Persistence.DBContext;

namespace TK_UR_BOOK.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext Contextt)
        {
            _context = Contextt;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await _dbSet.AddAsync(entity);
            }
        }

        public async Task<T?> GetByIdAsync<TId>(TId id) where TId : notnull
        => await _dbSet.FindAsync(id);

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] incloudes)
        {
            IQueryable<T> query = _dbSet;
            if (incloudes != null)
            {
                foreach (var include in incloudes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? incloud = null)
        {
            IQueryable<T> query = _dbSet;
            if (incloud != null)
            {
                query = incloud(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.AsNoTracking();
        }

        public void Remove(T entity)

          => _dbSet.Remove(entity);


        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Remove(entity);
            }
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
