using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Pages.Paciente
{
    public class EditarDatosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditarDatosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Usuario? Usuario { get; set; } // Permitir valores nulos

        public IActionResult OnGet(string documento)
        {
            Usuario = _context.Usuarios.FirstOrDefault(u => u.Documento == documento);

            if (Usuario == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Usuario == null) // Validar si Usuario es null
            {
                return BadRequest("El usuario no puede ser nulo.");
            }

            var usuarioDb = _context.Usuarios.FirstOrDefault(u => u.Documento == Usuario.Documento);

            if (usuarioDb == null)
            {
                return NotFound();
            }

            // Actualizar los datos del usuario
            usuarioDb.Celular = Usuario.Celular;
            usuarioDb.Correo = Usuario.Correo;
            usuarioDb.Direccion = Usuario.Direccion;
            usuarioDb.Ciudad = Usuario.Ciudad;

            _context.SaveChanges();

            TempData["Mensaje"] = "Datos actualizados correctamente.";
            return RedirectToPage("/Paciente/Calendario", new { documento = Usuario.Documento });
        }
    }
}