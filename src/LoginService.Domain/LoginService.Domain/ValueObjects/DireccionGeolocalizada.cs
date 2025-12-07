using Joseco.DDD.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.ValueObjects
{
    public record DireccionGeolocalizada 
    {
        public string DireccionCompleta { get; private set; }
        public double Latitud { get; private set; }
        public double Longitud { get; private set; }

        protected DireccionGeolocalizada() { }

        public DireccionGeolocalizada(string direccionCompleta, double latitud, double longitud)
        {
            if (latitud < -90 || latitud > 90)
            {
                var error = Error.Failure("Geo.LatitudInvalida", "La latitud '{lat}' debe estar entre -90 y 90.", latitud.ToString());
                throw new DomainException(error);
            }

            if (longitud < -180 || longitud > 180)
            {
                var error = Error.Failure("Geo.LongitudInvalida", "La longitud '{lon}' debe estar entre -180 y 180.", longitud.ToString());
                throw new DomainException(error);
            }
            if (string.IsNullOrWhiteSpace(direccionCompleta))
            {
                var error = Error.Failure("Geo.DireccionVacia", "La dirección de texto no puede estar vacía.");
                throw new DomainException(error);
            }
            DireccionCompleta = direccionCompleta;
            Latitud = latitud;
            Longitud = longitud;
        }
    }
}
