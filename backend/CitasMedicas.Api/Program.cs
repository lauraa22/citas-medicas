using CitasMedicas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Infrastructure.Repositories;

using CitasMedicas.Application.Mappings;

using CitasMedicas.Application.Interfaces.Services;
using CitasMedicas.Application.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se ha encontrado la cadena de conexión 'DefaultConnection'.");


builder.Services.AddDbContext<CitasMedicasDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



// Add services to the container.

builder.Services.AddAutoMapper(
    cfg => { },
    typeof(MappingProfile)
);

builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IDiagnosticoService, DiagnosticoService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Citas Médicas API v1"
        );
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
