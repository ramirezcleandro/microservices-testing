using Joseco.DDD.Core.Results;
using Logistica.Domain.Enums;

namespace LogisticaService.Domain.ValueObjects
{
    public record RegistroEntrega
    {
        public TipoPruebaEntrega TipoPrueba { get; } 
        public DateTime TimestampConfirmacion { get; }
        public DireccionGeolocalizada GeopointConfirmacion { get; }
        public string UrlEvidencia { get; }

        protected RegistroEntrega() { }

        public RegistroEntrega(TipoPruebaEntrega tipoPrueba, DateTime timestamp, DireccionGeolocalizada geopoint, string url)
        {
            if (timestamp == default)
            {
                var error = Error.Failure("Registro.TimestampVacio", "El timestamp de confirmación es obligatorio.");
                throw new DomainException(error);
            }
            if (geopoint == null)
            {
                var error = Error.Failure("Registro.GeopointNulo", "Se requiere el Geopoint de confirmación.");
                throw new DomainException(error);
            }

            TipoPrueba = tipoPrueba;
            TimestampConfirmacion = timestamp;
            GeopointConfirmacion = geopoint;
            UrlEvidencia = url;
        }

  
    }
}
