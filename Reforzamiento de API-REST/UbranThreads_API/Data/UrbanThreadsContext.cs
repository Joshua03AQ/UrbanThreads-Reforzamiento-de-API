using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace UbranThreads_API.Data
{
    public class UrbanThreadsContext : DbContext
    {
        public UrbanThreadsContext(DbContextOptions<UrbanThreadsContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaDetalle> VentaDetalles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuración de la relación entre Venta y VentaDetalle
            modelBuilder.Entity<Venta>()
                .HasMany(v => v.VentasDetalles)
                .WithOne(vd => vd.venta)
                .HasForeignKey(vd => vd.VentaID);
        }
    }
}
