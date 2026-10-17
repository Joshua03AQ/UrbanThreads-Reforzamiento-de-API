using SharedModels;

namespace UbranThreads_API.Repositories.IRepositories
{
    public interface IVentaRepository : IRepository<Venta>
    {
        Task<Venta> UpdateAsync(Venta venta);
    }
}
