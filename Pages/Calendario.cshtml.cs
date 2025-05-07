using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data;
using WebApp.Models;

namespace proyecto_integrador.Pages
{
    public class CalendarioModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CalendarioModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string Documento { get; set; } = string.Empty;
        public List<DiaModel> Dias { get; set; } = new List<DiaModel>();

        // Agregar la propiedad Usuario
        public Usuario Usuario { get; set; } = new Usuario();

        public void OnGet(string documento)
        {
            Documento = documento;

            // Obtener el usuario desde la base de datos
            Usuario = _context.Usuarios.FirstOrDefault(u => u.Documento == documento) ?? new Usuario();

            // Generar días de la semana con disponibilidad simulada
            var diasSemana = new[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
            var random = new Random();

            foreach (var dia in diasSemana)
            {
                var disponibilidad = random.Next(0, 101); // Simular disponibilidad entre 0% y 100%
                var color = disponibilidad > 40 ? "green" : disponibilidad > 0 ? "yellow" : "red";

                Dias.Add(new DiaModel
                {
                    Dia = dia,
                    Disponibilidad = disponibilidad,
                    Color = color
                });
            }
        }
    }
}