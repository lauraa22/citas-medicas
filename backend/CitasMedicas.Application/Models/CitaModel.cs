namespace CitasMedicas.Application.Models;

public class CitaModel
{
    public int Id { get; set; }

    public DateTime FechaHora { get; set; }

    public string MotivoCita { get; set; } = string.Empty;

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public int? DiagnosticoId { get; set; }
}