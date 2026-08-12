using CitasMedicas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);


var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se ha encontrado la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<CitasMedicasDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
