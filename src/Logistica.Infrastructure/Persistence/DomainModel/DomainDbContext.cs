using Joseco.DDD.Core.Abstractions;
using Logistica.Infrastructure.Persistence.DomainModel.Config;
using LogisticaService.Domain.Agregados;
using Microsoft.EntityFrameworkCore;

namespace Logistica.Infrastructure.Persistence.DomainModel
{
    public class DomainDbContext : DbContext
    {
        public DbSet<PaqueteEntrega> Paquetes { get; set; }
        public DbSet<RutaDistribucion> RutasDistribucion { get; set; }
        public DbSet<PuntoEntrega> PuntosEntrega { get; set; }

        public DomainDbContext(DbContextOptions<DomainDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore(typeof(DomainEvent));
            modelBuilder.Ignore(typeof(AggregateRoot));

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new RutaDistribucionConfig());
            modelBuilder.ApplyConfiguration(new PaqueteEntregaConfig());
        }
    }
}
