using CursoApis.Models;

namespace CursoApis.Services;

// Contrato del servicio encargado de trabajar con tareas.
public interface ITaskServices
{
    // Devuelve todas las tareas.
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    // Busca una tarea por Id.
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem?> DeleteByIdAsync(int id);
    Task<TaskItem?> UpdateByIdAsync(int id, TaskItem task);
    // Crea una tarea nueva.
    Task<TaskItem> CreateAsync(TaskItem task);
}