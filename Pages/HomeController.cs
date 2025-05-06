using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult BuscarDocumento(string documento)
    {
        if (EsEmpleado(documento))
        {
            return RedirectToAction("VistaEnfermero", new { documento });
        }
        else
        {
            return RedirectToAction("Calendario", new { documento });
        }
    }

    private bool EsEmpleado(string documento)
    {
        // Aquí puedes implementar la lógica para verificar si el documento es de un empleado.
        // Por ejemplo, podrías consultar una base de datos o una lista predefinida.
        var empleados = new List<string> { "12345", "67890" }; // Ejemplo de documentos de empleados
        return empleados.Contains(documento);
    }

    public IActionResult Calendario(string documento)
    {
        // Aquí se generará el calendario con las citas disponibles.
        ViewBag.Documento = documento;
        return View();
    }

    public IActionResult VistaEnfermero(string documento)
    {
        // Aquí se mostrarán las citas asignadas al enfermero.
        ViewBag.Documento = documento;
        return View();
    }
}