using System.ComponentModel.DataAnnotations;

namespace UbranThreads_API.Dto
{
    public class VentaDetalleCreateDto
    {
        //public int ID { get; set; }
        public int VentaID { get; set; }
        public int ProductoID { get; set; }
        [StringLength(50)]
        public string? TipoPrenda { get; set; }
        [StringLength(50)]
        public string? Marca { get; set; }
        public string? Talla { get; set; }
        public string? Color { get; set; }
        [Required]
        public string? Genero { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
