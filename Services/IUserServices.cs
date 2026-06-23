using CursoApis.Models;

namespace CursoApis.Services;

// Una interfaz define el contrato que una implementación debe cumplir.
// En este caso describe las operaciones disponibles para trabajar con usuarios.
public interface IUserService
{
    // Devuelve todos los usuarios.
    Task<IEnumerable<User>> GetAllUsersAsync();
    
    // Busca un usuario por su Id.
    Task<User?> GetByIdAsync(int id);
    
    Task<User?> DeleteByIdAsync(int id);
    
    Task<User?> UpdateByIdAsync(int id, User user);
    
    // Crea un nuevo usuario y lo guarda.
    Task<User> CreateAsync(User user);
}