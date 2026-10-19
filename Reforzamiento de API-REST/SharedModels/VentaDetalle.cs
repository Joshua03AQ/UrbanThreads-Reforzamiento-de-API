using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedModels
{
    public class VentaDetalle
    {
        public int ID { get; set; }
        public int VentaID { get; set; }
        [Column("ProductoID")]
        public int ProductID { get; set; }
        [StringLength(50)]
        public string? TipoPrenda { get; set; }
        [StringLength(50)]
        public string? Marca { get; set; }
        [StringLength(1)]
        public string? Talla { get; set; }
        public string? Color { get; set; }
        public string? Genero { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        public Venta? venta { get; set; }

    }
}