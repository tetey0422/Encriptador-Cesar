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

            // Asignar citas automáticamente si no están asignadas
            AsignarCitasEquitativamente();

            // Obtener las citas del día actual para el enfermero
            CitasDelDia = ObtenerCitasDelDia(documento);
        }

        private List<Cita> ObtenerCitasDelDia(string documentoEnfermero)
        {
            return _context.Citas
                .Where(c => c.EnfermeroDocumento == documentoEnfermero && c.Fecha.Date == DateTime.Now.Date)
                .OrderBy(c => c.Hora)
                .Select(c => new Cita // Mapear a la clase interna Cita
                {
                    Hora = c.Hora.ToString(@"hh\:mm"), // Convertir TimeSpan a string
                    Paciente = c.PacienteDocumento
                })
                .ToList();
        }

        private void AsignarCitasEquitativamente()
        {
            // Obtener todos los enfermeros activos
            var enfermerosActivos = _context.Usuarios
                .Where(u => u.EsEmpleado && !u.EsAdministrador)
                .Select(u => u.Documento)
                .ToList();

            if (!enfermerosActivos.Any())
            {
                return; // No hay enfermeros activos
            }

            // Obtener citas sin asignar para el día actual
            var citasSinAsignar = _context.Citas
                .Where(c => string.IsNullOrEmpty(c.EnfermeroDocumento) && c.Fecha.Date == DateTime.Now.Date)
                .OrderBy(c => c.Hora)
                .ToList();

            // Asignar citas de manera equitativa usando round-robin
            int index = 0;
            foreach (var cita in citasSinAsignar)
            {
                cita.EnfermeroDocumento = enfermerosActivos[index];
                index = (index + 1) % enfermerosActivos.Count; // Rotar entre los enfermeros
            }

            _context.SaveChanges();
        }

        public class Cita
        {
            public string Hora { get; set; } = string.Empty;
            public string Paciente { get; set; } = string.Empty;
        }
    }
}