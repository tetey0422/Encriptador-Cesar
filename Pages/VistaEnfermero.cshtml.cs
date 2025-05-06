using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data;
using System.Collections.Generic;
using System.Linq;

namespace proyecto_integrador.Pages
{
    public class VistaEnfermeroModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public VistaEnfermeroModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string Documento { get; set; } = string.Empty; // Documento del enfermero
        public List<Cita> CitasDelDia { get; set; } = new List<Cita>(); // Lista de citas del día

        public void OnGet(string documento)
        {
            var usuarioActual = _context.Usuarios.FirstOrDefault(u => u.Documento == documento);

            if (usuarioActual == null || !usuarioActual.EsEmpleado || usuarioActual.EsAdministrador)
            {
                Response.Redirect("/Error");
                return;
            }

            Documento = usuarioActual.Documento;
            CitasDelDia = ObtenerCitasDelDia(documento);
        }

        private List<Cita> ObtenerCitasDelDia(string documentoEnfermero)
        {
            return _context.Citas
                .Where(c => c.EnfermeroDocumento == documentoEnfermero && c.Fecha.Date == DateTime.Now.Date)
                .Select(c => new Cita
                {
                    Hora = c.Hora.ToString("HH:mm"), // Ajusta según el formato de hora
                    Paciente = c.PacienteDocumento
                })
                .ToList();
        }

        public class Cita
        {
            public string Hora { get; set; } = string.Empty;
            public string Paciente { get; set; } = string.Empty;
        }
    }
}