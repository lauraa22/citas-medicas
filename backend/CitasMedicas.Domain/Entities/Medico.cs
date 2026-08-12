namespace CitasMedicas.Domain.Entities;

public class Medico : Usuario
{
    public string NumColegiado { get; set; } = string.Empty;

    public ICollection<Paciente> Pacientes { get; set; }
        = new List<Paciente>();

    public ICollection<Cita> Citas { get; set; }
        = new List<Cita>();
}