namespace CitasMedicas.Application.DTOs.Medicos;

public class MedicoDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string NumColegiado { get; set; } = string.Empty;

    public List<int> PacienteIds { get; set; } = [];
}