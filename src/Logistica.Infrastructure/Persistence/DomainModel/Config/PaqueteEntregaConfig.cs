using LogisticaService.Domain.Agregados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistica.Infrastructure.Persistence.DomainModel.Config
{
    public class PaqueteEntregaConfig : IEntityTypeConfiguration<PaqueteEntrega>
    {
        public void Configure(EntityTypeBuilder<PaqueteEntrega> builder)
        {
            builder.ToTable("PaquetesEntrega");

            // 🔑 Clave primaria
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .IsRequired();

            // 🎫 Identificador de la etiqueta (único opcional)
            builder.Property(p => p.EtiquetaId)
                .HasColumnName("EtiquetaId")
                .HasMaxLength(50)
                .IsRequired();

            // 🔄 Estado del paquete (enum → string)
            builder.Property(p => p.EstadoPaquete)
                .HasConversion<string>()
                .HasColumnName("EstadoPaquete")
                .HasMaxLength(20)
                .IsRequired();

            // 📍 Propiedad de valor: Dirección de destino
            builder.OwnsOne(p => p.DireccionGeolocalizada, direccionBuilder =>
            {
                direccionBuilder.Property(d => d.DireccionCompleta)
                    .HasColumnName("Destino_DireccionCompleta")
                    .HasMaxLength(250)
                    .IsRequired();

                direccionBuilder.Property(d => d.Latitud)
                    .HasColumnName("Destino_Latitud");

                direccionBuilder.Property(d => d.Longitud)
                    .HasColumnName("Destino_Longitud");

                direccionBuilder.WithOwner();
            });

            // 🧾 Propiedad de valor: Registro de entrega
            builder.OwnsOne(p => p.RegistroEntrega, registroBuilder =>
            {
                registroBuilder.Property(r => r.TimestampConfirmacion)
                    .HasColumnName("Registro_TimestampConfirmacion");

                registroBuilder.Property(r => r.TipoPrueba)
                    .HasConversion<string>()
                    .HasColumnName("Registro_TipoPrueba")
                    .HasMaxLength(50);

                registroBuilder.Property(r => r.UrlEvidencia)
                    .HasColumnName("Registro_UrlEvidencia")
                    .HasMaxLength(500);

                // 📍 Composición dentro del registro: Geolocalización de confirmación
                registroBuilder.OwnsOne(r => r.GeopointConfirmacion, geopointBuilder =>
                {
                    geopointBuilder.Property(g => g.Latitud)
                        .HasColumnName("Registro_Latitud");

                    geopointBuilder.Property(g => g.Longitud)
                        .HasColumnName("Registro_Longitud");

                    geopointBuilder.Ignore(g => g.DireccionCompleta);
                });

                registroBuilder.WithOwner();
            });

            // 🚫 Ignorar eventos de dominio internos
            builder.Ignore("_domainEvents");

            // (Opcional) Índice por etiqueta para búsquedas más rápidas
            builder.HasIndex(p => p.EtiquetaId)
                .HasDatabaseName("IX_PaquetesEntrega_EtiquetaId");
        }
    }
}
