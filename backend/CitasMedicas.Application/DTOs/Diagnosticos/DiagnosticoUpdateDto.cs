namespace CitasMedicas.Application.DTOs.Diagnosticos;

/// <summary>
/// Representa los datos necesarios para actualizar un diagnóstico.
/// </summary>
public class DiagnosticoUpdateDto
{
    public string ValoracionEspecialista { get; set; } = string.Empty;

    public string Enfermedad { get; set; } = string.Empty;
}