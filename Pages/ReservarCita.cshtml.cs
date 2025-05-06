using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ReservarCitaModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Dia { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Documento { get; set; } = string.Empty;

    public string Mensaje { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        // Aquí podrías agregar lógica para verificar si la cita aún está disponible.
        Mensaje = $"¿Deseas confirmar tu cita para el día {Dia}?";
        return Page();
    }

    public IActionResult OnPost()
    {
        // Lógica para reservar la cita (actualizar la base de datos o lista en memoria).
        // Por ahora, redirigimos al calendario con un mensaje de éxito.
        return RedirectToPage("Calendario", new { documento = Documento });
    }
}