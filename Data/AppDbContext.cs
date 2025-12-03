using CitasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CitasApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paciente>().ToTable("catPacientes");
            modelBuilder.Entity<Medico>().ToTable("catMedicos");
            modelBuilder.Entity<Cita>().ToTable("citas");

            base.OnModelCreating(modelBuilder);
        }
    }
}