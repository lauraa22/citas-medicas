namespace CitasMedicas.Application.DTOs.Diagnosticos;

public class DiagnosticoDto
{
    public int Id { get; set; }

    public string ValoracionEspecialista { get; set; } = string.Empty;

    public string Enfermedad { get; set; } = string.Empty;
}