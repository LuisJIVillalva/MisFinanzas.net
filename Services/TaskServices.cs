using CursoApis.Data;
using CursoApis.Models;
using Microsoft.EntityFrameworkCore;


namespace CursoApis.Services
{
    // Implementación del contrato ITaskServices.
    // Este servicio encapsula el acceso a tareas en la base de datos.
    public class TaskServices : ITaskServices
    {
        // DbContext inyectado para operar con TaskItems.
        private readonly AppDbContext _context;

        public TaskServices(AppDbContext context)
        {
            _context = context;
        }

        // Agrega la tarea y persiste los cambios.
        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        // Edita la tarea y persiste los cambios.
        public async Task<TaskItem?> UpdateByIdAsync(int id, TaskItem pTask)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
                return null;
            _context.Entry(task).CurrentValues.SetValues(pTask);
            await _context.SaveChangesAsync();
            return task;
        }

        // Elimina la tarea y persiste los cambios.
        public async Task<TaskItem?> DeleteByIdAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
                return null;
            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return task;
        }

        // Devuelve todas las tareas sin tracking para lectura.
        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _context.TaskItems.AsNoTracking().ToListAsync();
        }

        // Busca una tarea por su identificador.
        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.TaskItems.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}