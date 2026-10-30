using System.ComponentModel.DataAnnotations;

namespace MediSafe.Models;

public class Consulta
{
    public int Id { get; set; }

    [Required]
    public int PacienteId { get; set; }

    public Paciente? Paciente { get; set; }

    [Required]
    public string MedicoId { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de consulta es obligatoria.")]
    public DateTime FechaConsulta { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El motivo de consulta es obligatorio.")]
    [StringLength(500)]
    public string Motivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El diagnóstico es obligatorio.")]
    [StringLength(1000)]
    public string Diagnostico { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Tratamiento { get; set; }

    [StringLength(2000)]
    public string? Observaciones { get; set; }
}