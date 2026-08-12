namespace CitasMedicas.Domain.Entities;

public abstract class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string NombreUsuario { get; set; } = string.Empty;

    public string Clave { get; set; } = string.Empty;
}