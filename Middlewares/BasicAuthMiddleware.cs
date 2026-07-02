using System.Net.Http.Headers;
using System.Text;

namespace CursoApis.Middlewares;

// Un middleware se ejecuta en cada request antes de llegar al controlador.
// Este middleware valida un usuario y contraseña enviados con Basic Auth.
public class BasicAuthMiddleware
{
    // Usuario y contraseña "hardcodeados" para el curso
    private const string DemoUsername = "platzi";

    private const string DemoPassword = "12345";

    // _next representa el siguiente middleware del pipeline.
    private readonly RequestDelegate _next;

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // InvokeAsync es el método que ASP.NET ejecuta por cada request.
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var path = context.Request.Path.Value ?? string.Empty;

            // Dejamos pasar sin autenticación a Swagger, Scalar y OpenAPI
            // para poder consultar la documentación del proyecto.
            if (path.Contains("swagger")
                || path.Contains("scalar")
                || path.Contains("openapi"))
            {
                await _next(context);
                return;
            }

            // Leer encabezado Authorization
            var authHeaderValue = context.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authHeaderValue))
                throw new InvalidOperationException("Authorization header faltante.");

            var authHeader = AuthenticationHeaderValue.Parse(authHeaderValue);
            if (string.IsNullOrWhiteSpace(authHeader.Parameter))
                throw new InvalidOperationException("Authorization header inválido.");

            // Decodificar credenciales base64
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            if (credentials.Length < 2) throw new InvalidOperationException("Credenciales incompletas.");

            var username = credentials[0];
            var password = credentials[1];

            // Validar usuario/contraseña
            if (username == DemoUsername && password == DemoPassword)
            {
                // Usuario autenticado → continuar pipeline
                await _next(context);
                return;
            }
        }
        catch
        {
            // Si ocurre error significa que no vino el header correctamente
        }

        // Si llega aquí → no autorizado
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized: Missing or invalid credentials.");
    }
}

// Las clases de extensión ayudan a registrar un middleware con una sintaxis más limpia.
// Gracias a este método luego se puede escribir app.UseBasicAuth().
public static class BasicAuthExtensions
{
    public static IApplicationBuilder UseBasicAuth(this IApplicationBuilder app)
    {
        return app.UseMiddleware<BasicAuthMiddleware>();
    }
}