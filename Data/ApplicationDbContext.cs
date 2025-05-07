using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación entre Cita y Usuario (Paciente)
            modelBuilder.Entity<Cita>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(c => c.PacienteDocumento)
                .HasPrincipalKey(u => u.Documento)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre Cita y Usuario (Enfermero)
            modelBuilder.Entity<Cita>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(c => c.EnfermeroDocumento)
                .HasPrincipalKey(u => u.Documento)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}