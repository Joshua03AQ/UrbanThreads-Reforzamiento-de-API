using System.ComponentModel.DataAnnotations;

namespace UbranThreads_API.Dto
{
    public class LoginUsuarioDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
