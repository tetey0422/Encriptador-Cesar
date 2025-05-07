using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data; // Contexto de la base de datos
using WebApp.Models; // Modelos de la base de datos

namespace proyecto_integrador.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context; // Contexto de la base de datos

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [BindProperty]
    public string? Documento { get; set; } // Permitir valores nulos

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Documento))
        {
            ModelState.AddModelError(string.Empty, "Debe ingresar un número de documento.");
            return Page();
        }

        // Consultar la base de datos para obtener el usuario por su documento
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Documento == Documento);

        if (usuario == null)
        {
            ModelState.AddModelError(string.Empty, "Documento no encontrado.");
            return Page();
        }

        // Determinar el rol del usuario y redirigir a la página correspondiente
        if (usuario.EsAdministrador)
        {
            return RedirectToPage("GestionUsuarios");
        }
        else if (usuario.EsEmpleado)
        {
            return RedirectToPage("/models/Citas");
        }
        else
        {
            return RedirectToPage("Calendario");
        }
    }
}