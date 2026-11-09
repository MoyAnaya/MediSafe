using System.ComponentModel.DataAnnotations;
using MediSafe.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MediSafe.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombre { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Especialidad { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Paciente> RegistrosPacientes { get; set; }

        public DbSet<Consulta> RegistrosConsultas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Paciente>().ToTable("Pacientes");

            builder.Entity<Consulta>().ToTable("Consultas");

            // Relación del paciente con su médico
            builder.Entity<Paciente>()
                .HasOne(p => p.Medico)
                .WithMany()
                .HasForeignKey(p => p.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación del paciente con su recepcionista
            builder.Entity<Paciente>()
                .HasOne(p => p.Recepcionista)
                .WithMany()
                .HasForeignKey(p => p.RecepcionistaId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
