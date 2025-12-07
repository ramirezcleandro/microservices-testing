using Joseco.DDD.Core.Abstractions;
using Logistica.Application;
using Logistica.Application.RutaDistribucion.Queries.Common;
using Logistica.Infrastructure.Interfaces;
using Logistica.Infrastructure.Persistence;
using Logistica.Infrastructure.Persistence.DomainModel;
using Logistica.Infrastructure.Persistence.Repositories;
using Logistica.Infrastructure.ReadStores;
using Logistica.Infrastructure.Services;
using LogisticaService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Logistica.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplication();                     // MediatR de Application
        services.AddPersistence(configuration);        // DB + repos + UoW
        services.AddSingleton<IGeolocationService, GoogleMapsGeolocationService>();
        return services;
    }

    /*public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("logisticDatabase");

        services.AddDbContext<DomainDbContext>((sp, opt) =>
        {
            opt.UseNpgsql(cs, npgsql =>
            {
                npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

            var lf = sp.GetRequiredService<ILoggerFactory>();
            opt.UseLoggerFactory(lf);
            opt.EnableSensitiveDataLogging();
            opt.EnableDetailedErrors();
        });

        // Repos + UoW
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRutaDistribucionRepository, RutaDistribucionRepository>();
        services.AddScoped<IPaqueteEntregaRepository, PaqueteEntregaRepository>();
        services.AddScoped<IRutaReadStore, RutaReadStore>();

        return services;
    }*/
    public static IServiceCollection AddPersistence(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        // ✔ Leer cadena logisticDatabase desde Docker
        var cs = configuration.GetConnectionString("logisticDatabase");

        Console.WriteLine("▶ Connection string usada:");
        Console.WriteLine(cs);

        // 1️⃣ Registrar PersistenceDbContext
        services.AddDbContext<Logistica.Infrastructure.Persistence.PersistenceModel.PersistenceDbContext>(options =>
        {
            options.UseNpgsql(cs, npgsql =>
            {
                npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        // 2️⃣ NO registrar DomainDbContext con EF Core
        //    Si lo usás, NO debe tener migraciones ni EF.
        services.AddDbContext<DomainDbContext>(options =>
        {
            options.UseNpgsql(cs);
        });


        // Repos + UoW
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRutaDistribucionRepository, RutaDistribucionRepository>();
        services.AddScoped<IPaqueteEntregaRepository, PaqueteEntregaRepository>();
        services.AddScoped<IRutaReadStore, RutaReadStore>();
  

        return services;
    }

}
