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
        public bool EsAdministrador { get; set; }

        [StringLength(15)]
        public string Celular { get; set; } = string.Empty;

        [StringLength(50)]
        public string Correo { get; set; } = string.Empty;

        [StringLength(100)]
        public string Direccion { get; set; } = string.Empty;

        [StringLength(50)]
        public string Ciudad { get; set; } = string.Empty;
        
        [Required]
        public bool Activo { get; set; } = true; // Indica si el usuario está activo o suspendido
    }
}