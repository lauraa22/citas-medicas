using CitasMedicas.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CitasMedicas.IntegrationTests.Infrastructure;

/// <summary>
/// Levanta la API real para los tests y sustituye la conexión habitual
/// por una base SQL Server exclusiva para integración.
/// </summary>
public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private const string TestConnectionString =
        @"Server=.\SQLEXPRESS;Database=CitasMedicasTestDb;Trusted_Connection=True;TrustServerCertificate=True;";
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Eliminamos el DbContext registrado por Program.cs
            // para evitar usar CitasMedicasDb durante los tests.
            services.RemoveAll<
                DbContextOptions<CitasMedicasDbContext>>();

            services.RemoveAll<CitasMedicasDbContext>();

            // Registramos el mismo DbContext, pero apuntando
            // a una base de datos EXCLUSIVA para integración.
            services.AddDbContext<CitasMedicasDbContext>(
                options =>
                    options.UseSqlServer(
                        TestConnectionString));
        });
    }

    /// <summary>
    /// Deja CitasMedicasTestDb limpia antes de cada test.
    /// NUNCA utiliza ni elimina CitasMedicasDb.
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        using var scope =
            Services.CreateScope();

        var context =
            scope.ServiceProvider
                .GetRequiredService<CitasMedicasDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}
