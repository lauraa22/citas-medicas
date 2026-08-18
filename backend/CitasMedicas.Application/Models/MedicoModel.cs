namespace CitasMedicas.Application.Models;

public class MedicoModel
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string? Clave { get; set; }

    public string NumColegiado { get; set; } = string.Empty;

    public List<int> PacienteIds { get; set; } = [];
}