using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Documento { get; set; } = string.Empty;

        [Required]
        public bool EsEmpleado { get; set; }

        [Required]
        public bool EsAdministrador { get; set; } // Nuevo campo para identificar administradores
    }
}