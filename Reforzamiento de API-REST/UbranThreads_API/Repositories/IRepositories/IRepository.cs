using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace UbranThreads_API.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>>?filter = null, bool tracked = true);
        Task<T> GetByAsync(int id);
        Task DeleteAsync(T entity); 
        Task SaveChangesAsync();
        Task<bool> ExistAsync(Expression<Func<T, bool>>? filter = null);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
