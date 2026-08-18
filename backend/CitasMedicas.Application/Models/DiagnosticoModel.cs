namespace CitasMedicas.Application.Models;

public class DiagnosticoModel
{
    public int Id { get; set; }

    public string ValoracionEspecialista { get; set; } = string.Empty;

    public string Enfermedad { get; set; } = string.Empty;
}