using System.ComponentModel.DataAnnotations;

namespace UbranThreads_API.Dto
{
    public class ProductoDto
    {
        [Required]
        public int ProductoID { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required] 
        public string Marca { get; set; }
        [Required]
        [MaxLength(1)]
        public string Talla { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required]
        public decimal Costo { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public int Cantidad { get; set; }
        public DateTime Registro { get; set; }

        public int introducirStock(int cantidad)
        {
            return Cantidad += cantidad;
        }
    }
}
