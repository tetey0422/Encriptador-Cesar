using Microsoft.AspNetCore.Mvc.RazorPages;

public class CalendarioModel : PageModel
{
    public string Documento { get; set; } = string.Empty;
    public List<DiaCitas> Dias { get; set; } = new List<DiaCitas>();

    public void OnGet(string documento)
    {
        Documento = documento;
        Dias = GenerarCalendario();
    }

    private List<DiaCitas> GenerarCalendario()
    {
        var random = new Random();
        var dias = new List<DiaCitas>();

        for (int dia = 1; dia <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); dia++)
        {
            var disponibilidad = random.Next(0, 100); // Generar disponibilidad aleatoria
            dias.Add(new DiaCitas
            {
                Dia = dia,
                Disponibilidad = disponibilidad,
                Color = disponibilidad > 40 ? "green" : disponibilidad > 0 ? "orange" : "red"
            });
        }

        return dias;
    }

    public class DiaCitas
    {
        public int Dia { get; set; }
        public int Disponibilidad { get; set; }
        public string Color { get; set; } = string.Empty;
    }
}