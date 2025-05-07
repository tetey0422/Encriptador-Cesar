using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data;
using WebApp.Models;
using System.Linq;
using System.Collections.Generic;

namespace proyecto_integrador.Pages
{
    public class GestionUsuariosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GestionUsuariosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Usuario NuevoUsuario { get; set; } = new Usuario();

        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public void OnGet()
        {
            var documentoUsuario = User.Identity?.Name; // Verifica si User.Identity es null
            if (string.IsNullOrEmpty(documentoUsuario))
            {
                Response.Redirect("/Error");
                return;
            }

            var usuarioActual = _context.Usuarios.FirstOrDefault(u => u.Documento == documentoUsuario);
            if (usuarioActual == null || !usuarioActual.EsAdministrador)
            {
                Response.Redirect("/Error");
                return;
            }

            Usuarios = _context.Usuarios.ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Usuarios.Add(NuevoUsuario);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostToggleActivo(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Cambiar el estado de activo
            usuario.Activo = !usuario.Activo;
            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}