using CitasMedicas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using CitasMedicas.Domain.Interfaces.Repositories;
using CitasMedicas.Infrastructure.Repositories;

using CitasMedicas.Application.Mappings;

using CitasMedicas.Application.UseCases.Usuarios;
using CitasMedicas.Application.UseCases.Pacientes;
using CitasMedicas.Application.UseCases.Medicos;
using CitasMedicas.Application.UseCases.Citas;
using CitasMedicas.Application.UseCases.Diagnosticos;

using CitasMedicas.Api.Exceptions;
using CitasMedicas.Api.Mappings;


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
    typeof(MappingProfile),
    typeof(ApiMappingProfile)
);

builder.Services.AddScoped<GetUsuariosUseCase>();
builder.Services.AddScoped<GetUsuarioUseCase>();
builder.Services.AddScoped<CreateUsuarioUseCase>();
builder.Services.AddScoped<UpdateUsuarioUseCase>();
builder.Services.AddScoped<DeleteUsuarioUseCase>();

builder.Services.AddScoped<GetPacientesUseCase>();
builder.Services.AddScoped<GetPacienteUseCase>();
builder.Services.AddScoped<CreatePacienteUseCase>();
builder.Services.AddScoped<UpdatePacienteUseCase>();
builder.Services.AddScoped<DeletePacienteUseCase>();


builder.Services.AddScoped<GetMedicosUseCase>();
builder.Services.AddScoped<GetMedicoUseCase>();
builder.Services.AddScoped<CreateMedicoUseCase>();
builder.Services.AddScoped<UpdateMedicoUseCase>();
builder.Services.AddScoped<DeleteMedicoUseCase>();


builder.Services.AddScoped<GetCitasUseCase>();
builder.Services.AddScoped<GetCitaUseCase>();
builder.Services.AddScoped<CreateCitaUseCase>();
builder.Services.AddScoped<UpdateCitaUseCase>();
builder.Services.AddScoped<DeleteCitaUseCase>();


builder.Services.AddScoped<GetDiagnosticosUseCase>();
builder.Services.AddScoped<GetDiagnosticoUseCase>();
builder.Services.AddScoped<CreateDiagnosticoUseCase>();
builder.Services.AddScoped<UpdateDiagnosticoUseCase>();
builder.Services.AddScoped<DeleteDiagnosticoUseCase>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

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


app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
