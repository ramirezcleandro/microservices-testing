using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using LogisticaService.Domain.Enums;
using LogisticaService.Domain.Events;
using LogisticaService.Domain.ServiciosDominio;
using LogisticaService.Domain.ValueObjects;

namespace LogisticaService.Domain.Agregados
{
    public class PaqueteEntrega : AggregateRoot
    {
        public string EtiquetaId { get; private set; }
        public EstadoPaquete EstadoPaquete { get; private set; }
        public DireccionGeolocalizada DireccionGeolocalizada { get; private set; }
        public RegistroEntrega? RegistroEntrega { get; private set; }

        public PaqueteEntrega(Guid id) :base(id){
            
            EtiquetaId = default;
            DireccionGeolocalizada = null!; // null-forgiving operator
            EstadoPaquete = default;
        }

       
        public PaqueteEntrega(Guid id, string etiquetaId, DireccionGeolocalizada direccionGeolocalizada) : base(id)
        {
            if (direccionGeolocalizada == null)
                throw new ArgumentNullException(nameof(direccionGeolocalizada));
            if (etiquetaId == string.Empty)
                throw new DomainException(PaqueteEntregaErrors.EtiquetaIdVacia);


            EtiquetaId = etiquetaId;
            DireccionGeolocalizada = direccionGeolocalizada;
            EstadoPaquete = EstadoPaquete.Recibido;
            
        }

       
        public void ConfirmarEntrega(RegistroEntrega registro, IPoliticaTolerancia politicaValidacion, double distanciaEnMetros)
        {
            if (EstadoPaquete == EstadoPaquete.Entregado)
                throw new DomainException(PaqueteEntregaErrors.EntregaYaConfirmada);
            if (registro == null)
                throw new DomainException(PaqueteEntregaErrors.RegistroEntregaNulo);

            

            RegistroEntrega = registro;
            EstadoPaquete = EstadoPaquete.Entregado;

            VerificadorEntregaService.AuditarPruebaGeografica(this
                , politicaValidacion,
                distanciaEnMetros: distanciaEnMetros);

            AddDomainEvent(new EntregaConfirmadaEvent(this.Id, this.EtiquetaId, registro.TimestampConfirmacion));
        }


    
    }
}
