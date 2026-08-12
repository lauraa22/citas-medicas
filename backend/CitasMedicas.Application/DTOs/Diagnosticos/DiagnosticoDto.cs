namespace CitasMedicas.Application.DTOs.Diagnosticos;

/// <summary>
/// Representa los datos de un diagnóstico médico.
/// </summary>
public class DiagnosticoDto
{
    public int Id { get; set; }

    /// <summary>
    /// Valoración realizada por el especialista.
    /// </summary>
    public string ValoracionEspecialista { get; set; } = string.Empty;

    /// <summary>
    /// Enfermedad diagnosticada.
    /// </summary>
    public string Enfermedad { get; set; } = string.Empty;
}