
using System.ComponentModel.DataAnnotations;
using MediSafe.Data;

namespace MediSafe.Models;

public class Paciente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? FechaNacimiento { get; set; }

    [Phone(ErrorMessage = "Ingresa un teléfono válido.")]
    [StringLength(20)]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    [StringLength(256)]
    public string? Email { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Identificador del médico propietario del paciente
    [Required]
    public string MedicoId { get; set; } = string.Empty;

    public ApplicationUser? Medico { get; set; }
}
