using System.ComponentModel.DataAnnotations;

namespace UbranThreads_API.Dto
{
    public class RegisterUsuarioDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Apellido { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(12)]
        public string? Password { get; set; }
    }
}
