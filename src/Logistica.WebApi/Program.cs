using Logistica.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1️⃣ Muy importante: habilitar variables de entorno (para Docker)
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddAuthorization();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    try
    {
        //var domainDb = scope.ServiceProvider.GetRequiredService<Logistica.Infrastructure.Persistence.DomainModel.DomainDbContext>();
       // await domainDb.Database.MigrateAsync();

        // Si también usas el PersistenceDbContext, descomenta:
         var persistenceDb = scope.ServiceProvider.GetRequiredService<Logistica.Infrastructure.Persistence.PersistenceModel.PersistenceDbContext>();
         await persistenceDb.Database.MigrateAsync();

        Console.WriteLine("✅ Migraciones aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ No se pudieron aplicar las migraciones: {ex.Message}");
        // En producción podrías relanzar la excepción si quieres que el servicio no levante sin DB consistente:
        // throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
