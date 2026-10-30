using System.ComponentModel.DataAnnotations;

namespace MediSafe.Models;

public class CrearConsultaModel
{
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime FechaConsulta { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El motivo es obligatorio.")]
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