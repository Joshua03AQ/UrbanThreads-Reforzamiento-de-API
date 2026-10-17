using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedModels
{
    public class Usuario
    {
        private int Id { get; set; }
        [Required]
        [StringLength(50)]
        private string? Username { get; set; }
        private string? Nombre { get; set; }
        private string? Apellido { get; set; }
        [EmailAddress]
        [Required]
        private string? Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(12)]
        private string? PasswordHash { get; set; }
    }
}
