namespace CitasMedicas.Api.DTOs;

/// <summary>
/// Representa los datos de una cita médica que se intercambian
/// entre la API y el cliente.
/// </summary>
public class CitaDto
{
    /// <summary>
    /// Identificador único de la cita.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Fecha y hora previstas para la cita.
    /// </summary>
    public DateTime FechaHora { get; set; }

    /// <summary>
    /// Motivo de la cita médica.
    /// </summary>
    public string MotivoCita { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del paciente asociado a la cita.
    /// </summary>
    public int PacienteId { get; set; }

    /// <summary>
    /// Identificador del médico asociado a la cita.
    /// </summary>
    public int MedicoId { get; set; }

    /// <summary>
    /// Identificador del diagnóstico asociado a la cita.
    /// Puede ser nulo mientras la cita no tenga diagnóstico.
    /// </summary>
    public int? DiagnosticoId { get; set; }
}