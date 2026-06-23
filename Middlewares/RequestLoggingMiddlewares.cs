namespace CursoApis.Middlewares;

// Middleware de ejemplo para ver el recorrido de una petición por consola.
// Muestra la ruta al entrar y el código de estado al salir.
public class RequestLoggingMiddleware
{
    // Referencia al siguiente paso del pipeline.
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Este método se ejecuta en cada request.
    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"[Middleware] Request: {context.Request.Path}");
        await _next(context);
        Console.WriteLine($"[Middleware] Response: {context.Response.StatusCode}");
    }
}

// Método de extensión para registrar el middleware con app.UseRequestLogging().
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}