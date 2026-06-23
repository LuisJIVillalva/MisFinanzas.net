namespace CursoApis.Models;

// Esta entidad representa una tarea dentro del sistema.
// EF Core la mapeará a la tabla Tasks.
public class TaskItem
{
    // Clave primaria de la tarea.
    public int Id { get; set; }
    // Título o descripción corta de la tarea.
    public string Title { get; set; } = default!;
    // Indica si la tarea ya fue completada.
    public bool IsCompleted { get; set; }
    // Clave foránea hacia el usuario dueño de la tarea.
    public int UserId { get; set; }
    // Propiedad de navegación: permite acceder al usuario relacionado.
    public User? User { get; set; }
}