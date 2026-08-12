using CitasMedicas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitasMedicas.Infrastructure.Persistence;

public class CitasMedicasDbContext : DbContext
{
    public CitasMedicasDbContext(
        DbContextOptions<CitasMedicasDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Medico> Medicos => Set<Medico>();
    public DbSet<Cita> Citas => Set<Cita>();
    public DbSet<Diagnostico> Diagnosticos => Set<Diagnostico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUsuario(modelBuilder);
        ConfigurePaciente(modelBuilder);
        ConfigureMedico(modelBuilder);
        ConfigurePacienteMedico(modelBuilder);
        ConfigureCita(modelBuilder);
        ConfigureDiagnostico(modelBuilder);
    }

    private static void ConfigureUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Apellidos)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<Usuario>()
            .Property(u => u.NombreUsuario)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.NombreUsuario)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Clave)
            .HasMaxLength(200)
            .IsRequired();
    }

    private static void ConfigurePaciente(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Paciente>()
            .Property(p => p.NSS)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Paciente>()
            .HasIndex(p => p.NSS)
            .IsUnique();

        modelBuilder.Entity<Paciente>()
            .Property(p => p.NumTarjeta)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Paciente>()
            .Property(p => p.Telefono)
            .HasMaxLength(20);

        modelBuilder.Entity<Paciente>()
            .Property(p => p.Direccion)
            .HasMaxLength(250);
    }

    private static void ConfigureMedico(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medico>()
            .Property(m => m.NumColegiado)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Medico>()
            .HasIndex(m => m.NumColegiado)
            .IsUnique();
    }

    private static void ConfigurePacienteMedico(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Paciente>()
            .HasMany(p => p.Medicos)
            .WithMany(m => m.Pacientes)
            .UsingEntity(j =>
                j.ToTable("PacienteMedico"));
    }

    private static void ConfigureCita(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Cita>()
            .Property(c => c.MotivoCita)
            .HasMaxLength(500)
            .IsRequired();

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Paciente)
            .WithMany(p => p.Citas)
            .HasForeignKey(c => c.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Medico)
            .WithMany(m => m.Citas)
            .HasForeignKey(c => c.MedicoId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureDiagnostico(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diagnostico>()
            .HasKey(d => d.Id);

        modelBuilder.Entity<Diagnostico>()
            .Property(d => d.ValoracionEspecialista)
            .HasMaxLength(1000);

        modelBuilder.Entity<Diagnostico>()
            .Property(d => d.Enfermedad)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Diagnostico)
            .WithOne(d => d.Cita)
            .HasForeignKey<Cita>(c => c.DiagnosticoId)
            .IsRequired(false);
    }
}