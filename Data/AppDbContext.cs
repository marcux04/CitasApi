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
            base.OnModelCreating(modelBuilder);

            // Configurar entidad Paciente
            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.ToTable("catPacientes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100);
                entity.Property(e => e.Curp).HasColumnName("curp").HasMaxLength(18);
                entity.Property(e => e.Telefono).HasColumnName("telefono").HasMaxLength(10);
                entity.Property(e => e.Correo).HasColumnName("correo").HasMaxLength(100);
                entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(255);
                
                // Índices únicos
                entity.HasIndex(e => e.Curp).IsUnique();
                entity.HasIndex(e => e.Correo).IsUnique();
            });

            // Configurar entidad Medico
            modelBuilder.Entity<Medico>(entity =>
            {
                entity.ToTable("catMedicos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100);
                entity.Property(e => e.Especialidad).HasColumnName("especialidad").HasMaxLength(50);
            });

            // Configurar entidad Cita
            modelBuilder.Entity<Cita>(entity =>
            {
                entity.ToTable("citas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PacienteId).HasColumnName("paciente_id");
                entity.Property(e => e.MedicoId).HasColumnName("medico_id");
                entity.Property(e => e.Fecha).HasColumnName("fecha").HasColumnType("date");
                entity.Property(e => e.Hora).HasColumnName("hora");
                entity.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(20).HasDefaultValue("pendiente");

                // Relaciones
                entity.HasOne(e => e.Paciente)
                    .WithMany(p => p.Citas)
                    .HasForeignKey(e => e.PacienteId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_citas_paciente");

                entity.HasOne(e => e.Medico)
                    .WithMany(m => m.Citas)
                    .HasForeignKey(e => e.MedicoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_citas_medico");
            });
        }
    }
}