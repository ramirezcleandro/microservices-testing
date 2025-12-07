using LogisticaService.Domain.Agregados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistica.Infrastructure.Persistence.DomainModel.Config
{
    public class PuntoEntregaConfig : IEntityTypeConfiguration<PuntoEntrega>
    {
        public void Configure(EntityTypeBuilder<PuntoEntrega> builder)
        {
            builder.ToTable("PuntosEntrega");

            
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                //.ValueGeneratedNever()
                .IsRequired();

            
            builder.Property(p => p.PaqueteId)
                .HasColumnName("PaqueteId")
                .IsRequired();

            
            builder.Property(p => p.Secuencia)
                .HasColumnName("Secuencia")
                .IsRequired();

            
            builder.Property(p => p.EstadoPunto)
                .HasConversion<string>()
                .HasColumnName("EstadoPunto")
                .HasMaxLength(20)
                .IsRequired();

            
            builder.Property(p => p.RutaDistribucionId)
                .HasColumnName("RutaDistribucionId")
                .IsRequired();

            builder.HasOne<RutaDistribucion>()          
                .WithMany("_puntos")                    
                .HasForeignKey(p => p.RutaDistribucionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.PaqueteId)
               .HasDatabaseName("IX_Punto_PaqueteId");


            builder.Ignore("_domainEvents");
        }
    }
}
