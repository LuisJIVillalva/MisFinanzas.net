namespace CursoApis.Models;

// Esta entidad representa un usuario del sistema.
// EF Core la mapeará a la tabla Users.
public class User
{
    // Clave primaria del usuario.
    public int Id { get; set; }
    // Nombre visible del usuario.
    public string Name { get; set; } = default!;
    // Correo electrónico del usuario.
    public string Email { get; set; } = default!;

    // Propiedad de navegación: un usuario puede tener muchas tareas.
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}