namespace CitasMedicas.Application.DTOs.Diagnosticos;

/// <summary>
/// Representa los datos necesarios para crear un diagnóstico.
/// </summary>
public class DiagnosticoCreateDto
{
    public string ValoracionEspecialista { get; set; } = string.Empty;

    public string Enfermedad { get; set; } = string.Empty;
}