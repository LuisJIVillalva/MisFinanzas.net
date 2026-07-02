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
            entity.ToTable("users");
            entity.HasKey(u => u.Id).HasName("pk_users");

            entity.Property(u => u.Id).HasColumnName("id");
            // Name y Email son obligatorios y además se limita su longitud máxima.
            entity.Property(u => u.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
            entity.Property(u => u.Email).HasColumnName("email").IsRequired().HasMaxLength(50);
        });

        // Configuración de la entidad TaskItem.
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("tasks");
            entity.HasKey(t => t.Id).HasName("pk_tasks");

            entity.Property(t => t.Id).HasColumnName("id");
            // Title es obligatorio y admite hasta 200 caracteres.
            entity.Property(t => t.Title).HasColumnName("title").IsRequired().HasMaxLength(200);
            // Si no se informa IsCompleted, por defecto arranca en false.
            entity.Property(t => t.IsCompleted).HasColumnName("is_completed").HasDefaultValue(false);
            entity.Property(t => t.UserId).HasColumnName("user_id");

            // Relación: una tarea pertenece a un usuario, y un usuario puede tener muchas tareas.
            entity.HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .HasConstraintName("fk_tasks_users_user_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(t => t.UserId).HasDatabaseName("ix_tasks_user_id");
        });
        
        
        // Configuración de la entidad Card.
        modelBuilder.Entity<Card>(entity =>
        {
            entity.ToTable("cards");
            entity.HasKey(c => c.Id).HasName("pk_cards");

            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.CardName).HasColumnName("card_name").IsRequired().HasMaxLength(100);
            
            entity.HasMany(c => c.Expenses)
                .WithOne(e => e.Card)
                .HasForeignKey(e => e.CardId)
                .HasConstraintName("fk_cards_expenses_card_id")
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Configuración de la entidad Summary.
        modelBuilder.Entity<Summary>(entity =>
        {
            entity.ToTable("summaries");
            entity.HasKey(s => s.Id).HasName("pk_summaries");

            entity.Property(s => s.Id).HasColumnName("id");
            entity.Property(s => s.Date).HasColumnName("date").IsRequired();
        });
        
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("expenses");
            entity.HasKey(e => e.Id).HasName("pk_expenses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Date).HasColumnName("date").IsRequired();
            entity.Property(e => e.SummaryId).HasColumnName("summary_id");
            entity.Property(e => e.Amount).HasColumnName("amount").IsRequired();
            entity.Property(e => e.NumberOfInstallments).HasColumnName("number_of_installments").HasDefaultValue(1);
            entity.Property(e => e.CardId).HasColumnName("card_id");

            entity.HasOne(e => e.Card)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CardId)
                .HasConstraintName("fk_expenses_cards_card_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Summary)
                .WithMany(s => s.Expenses)
                .HasForeignKey(e => e.SummaryId)
                .HasConstraintName("fk_expenses_summaries_summary_id")
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}