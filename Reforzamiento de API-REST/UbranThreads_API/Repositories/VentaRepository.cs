using SharedModels;
using UbranThreads_API.Data;
using UbranThreads_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace UbranThreads_API.Repositories
{
    public class VentaRepository : Repository<Venta>, IVentaRepository
    {
        private readonly UrbanThreadsContext? _context;

        public VentaRepository(UrbanThreadsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Venta> UpdateAsync(Venta venta)
        {
            _context.Entry(venta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return venta;
        }
    }
}
