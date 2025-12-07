using Logistica.Infrastructure.Persistence.PersistenceModel.EFCoreEntities;
using Microsoft.EntityFrameworkCore;

namespace Logistica.Infrastructure.Persistence.PersistenceModel
{
    public class PersistenceDbContext : DbContext, IDatabase
    {
        public DbSet<PaqueteEntregaPersistence> Paquetes { get; set; }
        public DbSet<RutaDistribucionPersistence> RutasDistribucion { get; set; }
        public DbSet<PuntoEntregaPersistence> PuntosEntrega { get; set; }

        public PersistenceDbContext(DbContextOptions<PersistenceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relaciones
            modelBuilder.Entity<RutaDistribucionPersistence>()
                .HasMany(r => r.PuntosEntrega)
                .WithOne(p => p.RutaDistribucion)
                .HasForeignKey(p => p.RutaDistribucionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración adicional si quieres índices
            modelBuilder.Entity<PaqueteEntregaPersistence>()
                .HasIndex(p => p.EtiquetaId)
                .HasDatabaseName("IX_Paquete_EtiquetaId");
        }

        public void Migrate()
        {
            Database.Migrate();
        }
    }
}
