namespace WebApp.Models
{
    public class DiaModel
    {
        public string Dia { get; set; } = string.Empty; // Nombre del día
        public int Disponibilidad { get; set; } // Porcentaje de disponibilidad
        public string Color { get; set; } = string.Empty; // Color basado en la disponibilidad
    }
}