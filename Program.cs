using System.Reflection;
using CursoApis.Data;
using CursoApis.Middlewares;
using CursoApis.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

// Este archivo es el punto de entrada de la API.
// Aquí se registran servicios (inyección de dependencias) y se arma el pipeline de middlewares.
var builder = WebApplication.CreateBuilder(args);

// Servicios básicos del proyecto.
// AddLogging permite usar ILogger<T> en controladores, servicios y middlewares.
builder.Services.AddLogging();
// AddControllers habilita que ASP.NET descubra y ejecute clases marcadas como Controller.
builder.Services.AddControllers();
// AddOpenApi expone el documento OpenAPI nativo de ASP.NET Core.
builder.Services.AddOpenApi();
// Registramos EF Core con una base en memoria para poder practicar sin una BD real.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("CursoApisDb");
});

// SwaggerGen genera la interfaz de Swagger y lee comentarios XML del código.
builder.Services.AddSwaggerGen(options =>
{
    // Ubica el archivo .xml generado por el proyecto para mostrar descripciones en Swagger.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Definimos un esquema BasicAuth para que Swagger permita enviar usuario/contraseña.
    options.AddSecurityDefinition("BasicAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization using user/name format."
    });

    // Indicamos que los endpoints pueden requerir el esquema BasicAuth definido arriba.
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("BasicAuth", document)] = []
    });
});

// Nombre de la política CORS que vamos a usar más abajo.
var myAllowOrigins = "MyAllowOrigins";

// CORS define desde qué orígenes se puede consumir esta API desde el navegador.
builder.Services.AddCors(options =>
    {
        options.AddPolicy(myAllowOrigins,
            p =>
            {
                // En este curso se permite cualquier encabezado y cualquier origen.
                // Es cómodo para desarrollo, pero en producción conviene restringirlo.
                p.AllowAnyHeader();
                p.AllowAnyOrigin();
            });
    }
);

builder.Services.AddCors();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskServices, TaskServices>();

// A partir de aquí se construye la aplicación con todo lo registrado antes.
var app = builder.Build();

// En desarrollo mostramos herramientas de documentación y prueba.
if (app.Environment.IsDevelopment())
{
    // Publica el documento OpenAPI y la referencia visual de Scalar.
    app.MapOpenApi();
    app.MapScalarApiReference();
    // Swagger UI permite probar endpoints desde el navegador.
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita la política CORS registrada antes.
app.UseCors(myAllowOrigins);

// Fuerza redirección a HTTPS.
app.UseHttpsRedirection();

// Middleware propio del curso para autenticación básica.
app.UseBasicAuth();

// Middleware de autorización del framework.
app.UseAuthorization();

// Middleware propio para registrar request y response en consola.
app.UseRequestLogging();

// Finalmente, mapea y expone los endpoints de los controladores.
app.MapControllers();

// Inicia la aplicación web.
app.Run();