using LogisticaService.Domain.Agregados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistica.Infrastructure.Persistence.DomainModel.Config
{
    public class RutaDistribucionConfig : IEntityTypeConfiguration<RutaDistribucion>
    {
        public void Configure(EntityTypeBuilder<RutaDistribucion> builder)
        {
            builder.ToTable("RutasDistribucion");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(r => r.Fecha)
                .HasColumnName("Fecha")
                .IsRequired();

            builder.Property(r => r.EstadoRuta)
                .HasConversion<string>()
                .HasColumnName("EstadoRuta")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(r => r.PersonalEntregaId)
                .HasColumnName("PersonalEntregaId")
                .IsRequired();

            
            builder.HasMany(r => r.Puntos)
                .WithOne()
                .HasForeignKey(p => p.RutaDistribucionId)
                .OnDelete(DeleteBehavior.Cascade);

          
            builder.Navigation(r => r.Puntos)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            
            builder.OwnsOne(r => r.AlmacenUbicacion, almacenBuilder =>
            {
                almacenBuilder.Property(a => a.DireccionCompleta)
                    .HasColumnName("Almacen_DireccionCompleta")
                    .HasMaxLength(250)
                    .IsRequired();

                almacenBuilder.Property(a => a.Latitud)
                    .HasColumnName("Almacen_Latitud");

                almacenBuilder.Property(a => a.Longitud)
                    .HasColumnName("Almacen_Longitud");

                almacenBuilder.WithOwner();
            });

            builder.HasIndex(r => new { r.PersonalEntregaId, r.EstadoRuta })
            .HasDatabaseName("IX_Ruta_Personal_Estado");

            builder.Ignore("_domainEvents");
        }
    }
}
