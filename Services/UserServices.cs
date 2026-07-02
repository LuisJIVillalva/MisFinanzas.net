using CursoApis.Data;
using CursoApis.Models;
using Microsoft.EntityFrameworkCore;

namespace CursoApis.Services;

// Un servicio concentra lógica de negocio o acceso a datos.
// Así evitamos meter toda la lógica dentro de los controladores.
public class UserService : IUserService

{
    // DbContext inyectado para consultar y guardar usuarios.
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    // Agrega un usuario a la base y confirma los cambios.
    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // Obtiene todos los usuarios sin seguimiento de cambios, ideal para lecturas.
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Tasks)
            .AsNoTracking()
            .ToListAsync();
    }

    // Busca el primer usuario cuyo Id coincida con el recibido.
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Elimina el primer usuario cuyo Id coincida con el recibido.
    public async Task<User?> DeleteByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
            return null;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // Actualiza el primer usuario cuyo Id coincida con el recibido.
    public async Task<User?> UpdateByIdAsync(int id, User pUser)
    {
        var user = await _context.Users
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return null;
        _context.Entry(user).CurrentValues.SetValues(pUser);
        await _context.SaveChangesAsync();

        return user;
    }
}