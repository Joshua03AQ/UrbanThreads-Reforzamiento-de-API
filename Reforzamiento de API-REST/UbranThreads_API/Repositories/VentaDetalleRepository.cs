using Microsoft.EntityFrameworkCore;
using SharedModels;
using UbranThreads_API.Data;
using UbranThreads_API.Repositories.IRepositories;

namespace UbranThreads_API.Repositories
{
    public class VentaDetalleRepository : Repository<VentaDetalle>, IVentaDetalleRepository
    {
        private readonly UrbanThreadsContext _context;

        public VentaDetalleRepository(UrbanThreadsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<VentaDetalle> UpdateAsync(VentaDetalle ventaDetalle)
        {
            _context.Entry(ventaDetalle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return ventaDetalle;
        }
    }
}
