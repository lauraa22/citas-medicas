namespace CitasMedicas.Application.DTOs.Citas;

public class CitaCreateDto
{
    public DateTime FechaHora { get; set; }

    public string MotivoCita { get; set; } = string.Empty;

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public int? DiagnosticoId { get; set; }
}