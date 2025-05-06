using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Services;

public class ValidarDocumentoModel : PageModel
{
    private readonly UsuarioService _usuarioService;

    public ValidarDocumentoModel(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [BindProperty]
    public string Documento { get; set; } = string.Empty;

    public IActionResult OnPost()
    {
        var usuario = _usuarioService.ObtenerUsuarioPorDocumento(Documento);

        if (usuario == null)
        {
            return RedirectToPage("Error");
        }

        if (usuario.EsAdministrador)
        {
            return RedirectToPage("GestionUsuarios");
        }
        else if (usuario.EsEmpleado)
        {
            return RedirectToPage("VistaEnfermero", new { documento = Documento });
        }
        else
        {
            return RedirectToPage("Calendario", new { documento = Documento });
        }
    }
}