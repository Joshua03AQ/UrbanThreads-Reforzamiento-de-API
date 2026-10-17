using SharedModels;

namespace UbranThreads_API.Repositories.IRepositories
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<Producto> UpdateAsync(Producto entity);
    }
}
