using System.ComponentModel.DataAnnotations;

namespace UbranThreads_API.Dto
{
    public class ProductoCreateDto
    {
        //[Required]
        //public int ProductoId { get; set; }
        [Required]
        [StringLength(50)]
        public string? Tipo { get; set; }
        [Required]
        [StringLength(50)]
        public string? Marca { get; set; }
        [Required]
        [StringLength(1)]
        public string? Talla { get; set; }
        [Required,StringLength(50)]
        public string? Color { get; set; }
        [Required]
        public string? Genero { get; set; }
        [Required]
        public decimal costo { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public int Cantidad { get; set; }
        public DateTime Registro { get; set;  }
    }
}
