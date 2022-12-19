using DesafioWeDecode.Model;
using Microsoft.EntityFrameworkCore;

namespace DesafioWeDecode.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts)
            : base(opts)
        {

        }

        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PacienteMedicamento>()
                .HasKey(x => new { x.PacienteId, x.MedicamentoId });

            modelBuilder.Entity<PacienteMedicamento>()
                .HasOne(x => x.Paciente)
                .WithMany(p => p.PacienteMedicamentos)
                .HasForeignKey(x => x.PacienteId);
        
            modelBuilder.Entity<PacienteMedicamento>()
                .HasOne(x => x.Medicamento)
                .WithMany(m => m.PacienteMedicamentos)
                .HasForeignKey(x => x.MedicamentoId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
