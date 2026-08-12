namespace CitasMedicas.Domain.Entities;

public class Diagnostico
{
    public int Id { get; set; }

    public string ValoracionEspecialista { get; set; }
        = string.Empty;

    public string Enfermedad { get; set; }
        = string.Empty;

    public Cita? Cita { get; set; }
}