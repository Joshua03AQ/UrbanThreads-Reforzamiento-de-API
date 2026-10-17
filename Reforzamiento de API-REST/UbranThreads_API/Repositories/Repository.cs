using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using UbranThreads_API.Data;
using UbranThreads_API.Repositories.IRepositories;

namespace UbranThreads_API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly UrbanThreadsContext? _context;
        internal DbSet<T>? dbSet { get; set;}

        public Repository(UrbanThreadsContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T>? query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.AnyAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T>? query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T>? query = dbSet;
            if(!tracked)
            {
                query = query.AsNoTracking();
            }
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetByAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
