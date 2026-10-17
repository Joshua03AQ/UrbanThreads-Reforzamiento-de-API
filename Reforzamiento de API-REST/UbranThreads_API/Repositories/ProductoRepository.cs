using Microsoft.EntityFrameworkCore;
using SharedModels;
using UbranThreads_API.Data;
using UbranThreads_API.Repositories.IRepositories;

namespace UbranThreads_API.Repositories
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly UrbanThreadsContext _context;

        public ProductoRepository(UrbanThreadsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Producto> UpdateAsync(Producto entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
