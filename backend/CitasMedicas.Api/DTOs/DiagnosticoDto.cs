namespace CitasMedicas.Api.DTOs;

/// <summary>
/// Representa los datos de un diagnóstico que se intercambian
/// entre la API y el cliente.
/// </summary>
public class DiagnosticoDto
{
    /// <summary>
    /// Identificador único del diagnóstico.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Valoración realizada por el especialista.
    /// </summary>
    public string ValoracionEspecialista { get; set; } = string.Empty;

    /// <summary>
    /// Enfermedad o diagnóstico determinado por el especialista.
    /// </summary>
    public string Enfermedad { get; set; } = string.Empty;
}