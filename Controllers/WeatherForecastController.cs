using Microsoft.AspNetCore.Mvc;

namespace CursoApis.Controllers;

// Un Controller recibe peticiones HTTP y devuelve respuestas.
// En este caso expone un CRUD simple en memoria para practicar endpoints.
[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    // Catálogo de textos posibles para la propiedad Summary.
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    // Lista estática en memoria.
    // Sirve para practicar, pero no reemplaza una base de datos real.
    private static List<WeatherForecast> ListWeatherForecast = [];

    // El constructor arma datos de ejemplo para poder probar el controlador rápidamente.
    public WeatherForecastController()
    {
        ListWeatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // Devuelve toda la lista actual de pronósticos.
        return ListWeatherForecast;
    }

    /// <summary>
    /// Retorna un elemento por su índice dentro de la lista en memoria.
    /// </summary>
    /// <param name="logger">Logger inyectado por ASP.NET para registrar eventos.</param>
    /// <param name="id">Posición del elemento dentro de la lista.</param>
    /// <returns>El pronóstico encontrado o un error si el índice no existe.</returns>
    [HttpGet]
    [Route("{id}")]
    public ActionResult<WeatherForecast> GetByPosition([FromServices] ILogger<WeatherForecastController> logger, int id)
    {
        // Ejemplo de uso de ILogger para registrar qué está ocurriendo.
        logger.LogInformation($"Getting weather forecast for {id}");
        // Validamos que el índice pedido exista dentro de la lista.
        if (id > ListWeatherForecast.Count || id < 0) return BadRequest();
        return Ok(ListWeatherForecast[id]);
    }

    // Actualiza un elemento existente según su posición dentro de la lista.
    [HttpPut]
    [Route("{id}")]
    public ActionResult<WeatherForecast> PutByPosition(int id, [FromBody] WeatherForecast forecast)
    {
        if (id > ListWeatherForecast.Count || id < 0) return BadRequest();
        ListWeatherForecast[id] = forecast;
        return Ok(ListWeatherForecast[id]);
    }


    // Crea un nuevo pronóstico y lo agrega al final de la lista en memoria.
    [HttpPost]
    public ActionResult<WeatherForecast> Post([FromBody] WeatherForecast forecast)
    {
        ListWeatherForecast.Add(forecast);
        return Ok(ListWeatherForecast);
    }


    // Elimina un pronóstico según su posición.
    [HttpDelete]
    [Route("{id}")]
    public ActionResult<WeatherForecast> DeleteByPosition(int id)
    {
        if (id >= ListWeatherForecast.Count || id < 0) return BadRequest();
        // Guardamos el valor antes de borrarlo para poder devolverlo en la respuesta.
        var deleted = ListWeatherForecast[id];
        ListWeatherForecast.RemoveAt(id);
        return Ok(deleted);
    }
}