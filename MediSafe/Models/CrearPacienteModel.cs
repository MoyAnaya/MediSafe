using System.ComponentModel.DataAnnotations;

namespace MediSafe.Models;

public class CrearPacienteModel
{
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
}

