using System.ComponentModel.DataAnnotations;

namespace SharedModels
{
    public class Producto
    {
        [Key]
        public int ProductID { get; set; }
        [StringLength(50)]
        public string? Tipo { get; set; }
        [StringLength(50)]
        public string? Marca { get; set; }
        [StringLength(1)]
        public string? Talla { get; set; }
        [StringLength(50)]
        public string? Color { get; set; }
        [StringLength(50)]
        public string? Genero { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public DateTime Registro { get; set; }

    }
}
