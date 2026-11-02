using System.ComponentModel.DataAnnotations;

namespace MediSafe.Models;

public class CrearPacienteModel
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100, ErrorMessage = "Los apellidos no pueden superar los 100 caracteres.")]
    public string Apellidos { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? FechaNacimiento { get; set; }

    [RegularExpression(@"^$|^\d{10}$",
        ErrorMessage = "El teléfono debe contener exactamente 10 dígitos, sin espacios ni símbolos.")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
    [StringLength(256)]
    public string? Email { get; set; }
}