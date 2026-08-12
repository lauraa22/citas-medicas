namespace CitasMedicas.Domain.Entities;

public class Paciente : Usuario
{
    public string NSS { get; set; } = string.Empty;

    public string NumTarjeta { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public ICollection<Medico> Medicos { get; set; }
        = new List<Medico>();

    public ICollection<Cita> Citas { get; set; }
        = new List<Cita>();
}