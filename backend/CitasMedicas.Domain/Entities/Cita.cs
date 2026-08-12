namespace CitasMedicas.Domain.Entities;

public class Cita
{
    public int Id { get; set; }

    public DateTime FechaHora { get; set; }

    public string MotivoCita { get; set; } = string.Empty;

    public int PacienteId { get; set; }

    public Paciente Paciente { get; set; } = null!;

    public int MedicoId { get; set; }

    public Medico Medico { get; set; } = null!;

    public int? DiagnosticoId { get; set; }

    public Diagnostico? Diagnostico { get; set; }
}