using Logistica.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7018); // HTTP → Pact
    options.ListenAnyIP(7019, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS → Swagger / normal
    });
});

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


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

//app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowReact");

app.UseAuthorization();

app.MapControllers();

app.Run();
