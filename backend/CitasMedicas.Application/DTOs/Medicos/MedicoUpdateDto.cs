namespace CitasMedicas.Application.DTOs.Medicos;

public class MedicoUpdateDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string Clave { get; set; } = string.Empty;

    public string NumColegiado { get; set; } = string.Empty;

    public List<int> PacienteIds { get; set; } = [];
}