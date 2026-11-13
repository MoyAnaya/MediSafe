using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using Ganss.Xss;
using MediSafe.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        private readonly CifradoService _cifrado;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            CifradoService cifrado)
            : base(options)
        {
            _cifrado = cifrado;
        }

        public DbSet<Paciente> RegistrosPacientes { get; set; }

        public DbSet<Consulta> RegistrosConsultas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Mantener las tablas existentes.
            builder.Entity<Paciente>().ToTable("Pacientes");

            builder.Entity<Consulta>().ToTable("Consultas");

            // Mantener las relaciones entre pacientes y médicos.
            builder.Entity<Paciente>()
                .HasOne(p => p.Medico)
                .WithMany()
                .HasForeignKey(p => p.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Paciente>()
                .HasOne(p => p.Recepcionista)
                .WithMany()
                .HasForeignKey(p => p.RecepcionistaId)
                .OnDelete(DeleteBehavior.SetNull);

            // ==========================================
            // CIFRADO DE DATOS DE LOS USUARIOS
            // ==========================================

            // Utilizar el servicio de cifrado.
            var cifrado = _cifrado;

            var conversionCifrado = new ValueConverter<string, string>(
                valor => cifrado.Cifrar(valor),
                valor => cifrado.Descifrar(valor)
            );

            // Cifrar el nombre de todos los usuarios.
            builder.Entity<ApplicationUser>()
                .Property(u => u.Nombre)
                .HasConversion(conversionCifrado)
                .HasColumnType("longtext");

            // Cifrar la especialidad de los médicos.
            var conversionEspecialidad =
            new ValueConverter<string?, string>(
                valor => cifrado.Cifrar(valor!),
                valor => cifrado.Descifrar(valor)
                );

            builder.Entity<ApplicationUser>()
                .Property(u => u.Especialidad)
                .HasConversion(conversionEspecialidad)
                .HasColumnType("longtext");
        }

        // ==========================================
        // VALIDACIÓN DE CONTENIDO HTML Y JAVASCRIPT
        // ==========================================

        private void ValidarContenidoTexto()
        {
            var sanitizador = new HtmlSanitizer();

            sanitizador.AllowedTags.Clear();
            sanitizador.AllowedAttributes.Clear();

            var entidades = ChangeTracker.Entries()
                .Where(e =>
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified);

            foreach (var entidad in entidades)
            {
                var tipo = entidad.Metadata.ClrType;

                bool esEntidadMediSafe =
                    tipo == typeof(ApplicationUser) ||
                    tipo.Namespace == typeof(Paciente).Namespace;

                if (!esEntidadMediSafe)
                {
                    continue;
                }

                foreach (var propiedad in entidad.Properties)
                {
                    if (propiedad.Metadata.ClrType != typeof(string))
                    {
                        continue;
                    }

                    var texto = propiedad.CurrentValue as string;

                    if (string.IsNullOrWhiteSpace(texto))
                    {
                        continue;
                    }

                    // Decodificar entidades HTML.
                    var textoDecodificado = texto;

                    for (int i = 0; i < 3; i++)
                    {
                        var siguiente =
                            WebUtility.HtmlDecode(textoDecodificado);

                        if (siguiente == textoDecodificado)
                        {
                            break;
                        }

                        textoDecodificado = siguiente;
                    }

                    var textoLimpio =
                        sanitizador.Sanitize(textoDecodificado);

                    bool contieneHtml =
                        !string.Equals(
                            textoDecodificado,
                            textoLimpio,
                            StringComparison.Ordinal);

                    bool contieneProtocoloPeligroso =
                        Regex.IsMatch(
                            textoDecodificado,
                            @"\b(javascript|vbscript)\s*:",
                            RegexOptions.IgnoreCase);

                    if (contieneHtml || contieneProtocoloPeligroso)
                    {
                        throw new ContenidoNoPermitidoException();
                    }
                }
            }
        }

        // Validar antes de guardar cambios.
        public override int SaveChanges(
            bool acceptAllChangesOnSuccess)
        {
            ValidarContenidoTexto();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        // Validar antes de guardar cambios de forma asíncrona.
        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ValidarContenidoTexto();

            return base.SaveChangesAsync(
                acceptAllChangesOnSuccess,
                cancellationToken);
        }
    }
}
