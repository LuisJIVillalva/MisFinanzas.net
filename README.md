# CursoApis

Este proyecto es una API hecha con **ASP.NET Core** y **Entity Framework Core** usando una base de datos en memoria.

La idea de este README es que te sirva como guía de estudio mientras avanzas en el curso.

---

## Cómo está organizado el proyecto

### `Program.cs`

Es el punto de entrada de la aplicación.

Ahí se hace todo esto:

- se registran servicios en el contenedor de dependencias
- se configura Entity Framework Core
- se configura Swagger / OpenAPI
- se activa CORS
- se agregan middlewares
- se publican los controladores

Piensa en `Program.cs` como el archivo que **arma la aplicación**.

---

### `Controllers/`

Los controladores reciben las peticiones HTTP.

Ejemplo:

- `GET`
- `POST`
- `PUT`
- `DELETE`

Un controlador normalmente:

1. recibe la petición
2. valida datos
3. llama a un servicio
4. devuelve una respuesta HTTP

---

### `Models/`

Aquí están las clases que representan los datos del sistema.

En este proyecto:

- `User` representa un usuario
- `TaskItem` representa una tarea
- `WeatherForecast` representa un pronóstico de clima

Estas clases después se usan en:

- controladores
- servicios
- Entity Framework
- respuestas JSON

---

### `Data/AppDbContext.cs`

Es la clase que representa la conexión lógica con la base de datos.

Define:

- qué entidades existen (`Users`, `TaskItems`)
- cómo se relacionan entre sí
- restricciones como campos obligatorios o largos máximos

Piensa en `AppDbContext` como el archivo donde le explicas a Entity Framework:

> "Estas son mis tablas y así se comportan"

---

### `Services/`

Los servicios contienen la lógica de acceso a datos o reglas del negocio.

La idea es **no meter toda la lógica en el controlador**.

Ejemplo:

- el controlador recibe la petición
- el servicio hace el trabajo real con la base de datos

Esto ayuda a que el código quede:

- más ordenado
- más reutilizable
- más fácil de mantener

---

### `Middlewares/`

Los middlewares son pasos por los que pasa cada request antes de llegar al controlador.

Ejemplo en este proyecto:

- `BasicAuthMiddleware` valida usuario y contraseña
- `RequestLoggingMiddleware` registra en consola qué request entró y qué respuesta salió

Piensa en ellos como filtros o guardias que revisan la petición en el camino.

---

### `appsettings.json`

Guarda configuración de la aplicación.

Por ejemplo:

- niveles de logging
- hosts permitidos
- cadenas de conexión en proyectos más grandes

> Nota: no agregué comentarios dentro de los archivos `json` porque JSON no soporta comentarios y se rompería el
> archivo.

---

## Paso a paso: cómo crear un servicio

Voy a explicarlo con la forma en que está armado este proyecto.

---

### 1. Crear o identificar el modelo

Primero necesitas una entidad sobre la que vas a trabajar.

Ejemplo:

- `User`
- `TaskItem`

Archivo de ejemplo:

- `Models/User.cs`
- `Models/TaskItem.cs`

Ejemplo simple:

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}
```

---

### 2. Agregar la entidad al `AppDbContext`

Después debes decirle a Entity Framework que esa entidad existe.

En `Data/AppDbContext.cs`:

```csharp
public DbSet<User> Users => Set<User>();
```

Y opcionalmente configurar reglas en `OnModelCreating`:

```csharp
modelBuilder.Entity<User>(entity =>
{
    entity.ToTable("Users");
    entity.HasKey(u => u.Id);
    entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
    entity.Property(u => u.Email).IsRequired().HasMaxLength(50);
});
```

---

### 3. Crear la interfaz del servicio

Primero se define el contrato.

Ejemplo en `Services/IUserServices.cs`:

```csharp
public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
}
```

La interfaz responde esta pregunta:

> "¿Qué cosas sabe hacer este servicio?"

---

### 4. Crear la implementación del servicio

Ahora se crea la clase real que implementa la interfaz.

Ejemplo en `Services/UserServices.cs`:

```csharp
public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
```

### Qué hace cada parte

- `private readonly AppDbContext _context;`
    - guarda la referencia a la base de datos
- constructor con `AppDbContext`
    - recibe el contexto por inyección de dependencias
- `AsNoTracking()`
    - mejora rendimiento cuando solo lees
- `ToListAsync()`
    - ejecuta la consulta y devuelve una lista
- `FirstOrDefaultAsync()`
    - busca el primer elemento o devuelve `null`
- `SaveChangesAsync()`
    - guarda cambios en la base de datos

---

### 5. Registrar el servicio en `Program.cs`

Si no registras el servicio, ASP.NET no podrá inyectarlo.

Ejemplo:

```csharp
builder.Services.AddScoped<IUserService, UserService>();
```

Y para tareas:

```csharp
builder.Services.AddScoped<ITaskServices, TaskServices>();
```

### Qué significa `AddScoped`

Significa que se crea una instancia del servicio por cada request HTTP.

Es la opción más común para servicios que usan `DbContext`.

---

### 6. Usar el servicio en un controlador

Una vez registrado, puedes pedirlo en el constructor del controlador.

Ejemplo:

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
}
```

---

### 7. Probarlo en Swagger

Cuando levantes el proyecto, puedes abrir Swagger y probar los endpoints.

Comandos útiles:

```bash
dotnet clean
dotnet build
dotnet watch run
```

---

## Resumen mental rápido

Si quieres crear un servicio nuevo, piensa así:

1. creo el modelo
2. lo agrego al `DbContext`
3. creo la interfaz
4. creo la implementación
5. lo registro en `Program.cs`
6. lo uso desde un controlador

---

## Ejemplo mental con una entidad nueva

Si mañana quieres agregar `Product`, harías esto:

1. crear `Models/Product.cs`
2. agregar `DbSet<Product>` en `AppDbContext`
3. crear `IProductService`
4. crear `ProductService`
5. registrar en `Program.cs`
6. crear `ProductsController`

---

## Cosas importantes para entender mejor

### Controlador

Habla HTTP.

### Servicio

Hace trabajo de negocio o acceso a datos.

### DbContext

Habla con la base de datos.

### Modelo

Representa datos.

### Middleware

Interviene en cada request antes o después del controlador.

---

## Sugerencia para estudiar este proyecto

Te conviene leer los archivos en este orden:

1. `Program.cs`
2. `Models/User.cs`
3. `Models/TaskItem.cs`
4. `Data/AppDbContext.cs`
5. `Services/IUserServices.cs`
6. `Services/UserServices.cs`
7. `Services/ITaskServices.cs`
8. `Services/TaskServices.cs`
9. `Controllers/WeatherForecastController.cs`
10. `Middlewares/BasicAuthMiddleware.cs`
11. `Middlewares/RequestLoggingMiddlewares.cs`

Ese orden ayuda porque va de lo más general a lo más concreto.

---

## Nota final

Este proyecto tiene partes pensadas para aprender:

- base en memoria
- autenticación básica simple
- controlador de clima con lista en memoria

Eso está bien para estudiar.
Más adelante normalmente se mejora con:

- base de datos real
- servicios registrados en DI
- controladores separados por entidad
- validaciones más prolijas
- autenticación más robusta

