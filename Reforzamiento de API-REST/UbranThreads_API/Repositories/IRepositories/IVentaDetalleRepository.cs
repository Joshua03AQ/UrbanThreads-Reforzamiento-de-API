using SharedModels;

namespace UbranThreads_API.Repositories.IRepositories
{
    public interface IVentaDetalleRepository : IRepository<VentaDetalle>
    {
        Task<VentaDetalle> UpdateAsync(VentaDetalle ventaDetalle);
    }
}
