namespace CitasMedicas.Application.Models;

public class PacienteModel
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string? Clave { get; set; }

    public string NSS { get; set; } = string.Empty;

    public string NumTarjeta { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public List<int> MedicoIds { get; set; } = [];
}