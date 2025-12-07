using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace Logistica.Infrastructure.Persistence.PersistenceModel
{
    public class PersistenceDbContextFactory : IDesignTimeDbContextFactory<PersistenceDbContext>
    {
        public PersistenceDbContext CreateDbContext(string[] args)
        {
            // 📁 Buscar el appsettings.json del WebApi (proyecto de inicio)
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Logistica.WebApi");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 🔌 Obtener la cadena de conexión
            var connectionString = configuration.GetConnectionString("logisticDatabase");

            // 🧩 Crear opciones del contexto
            var optionsBuilder = new DbContextOptionsBuilder<PersistenceDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new PersistenceDbContext(optionsBuilder.Options);
        }
    }
}
