namespace CursoApis;

// Modelo simple usado por el ejemplo inicial del template de ASP.NET.
// Representa un pronóstico del clima que luego devuelve el controlador.
public class WeatherForecast
{
    // Fecha para la cual aplica el pronóstico.
    public DateOnly Date { get; set; }

    // Temperatura en grados Celsius.
    public int TemperatureC { get; set; }

    // Propiedad calculada: no se guarda, se obtiene a partir de TemperatureC.
    // La fórmula convierte Celsius a Fahrenheit.
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    // Texto descriptivo opcional, por ejemplo: Warm, Hot, Chilly, etc.
    public string? Summary { get; set; }
}