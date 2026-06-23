using CursoApis.Models;
using Microsoft.EntityFrameworkCore;

namespace CursoApis.Data;

// DbContext representa la sesión de trabajo con la base de datos.
// EF Core usa esta clase para saber qué entidades existen y cómo mapearlas.
public class AppDbContext : DbContext
{
    // El framework inyecta opciones como el proveedor de BD (en este caso InMemoryDatabase).
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    // Cada DbSet representa una tabla o colección de entidades.
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    // OnModelCreating permite configurar restricciones y relaciones con Fluent API.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la entidad User.
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            // Name y Email son obligatorios y además se limita su longitud máxima.
            entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(50);
        });

        // Configuración de la entidad TaskItem.
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");
            entity.HasKey(t => t.Id);
            // Title es obligatorio y admite hasta 200 caracteres.
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            // Si no se informa IsCompleted, por defecto arranca en false.
            entity.Property(t => t.IsCompleted).HasDefaultValue(false);

            // Relación: una tarea pertenece a un usuario, y un usuario puede tener muchas tareas.
            entity.HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}