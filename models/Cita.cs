using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required] // Nueva propiedad
        public TimeSpan Hora { get; set; }

        [Required]
        [StringLength(10)]
        public string PacienteDocumento { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string EnfermeroDocumento { get; set; } = string.Empty;
    }
}