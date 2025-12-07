using Joseco.DDD.Core.Abstractions;
using LogisticaService.Domain.Events;

namespace LogisticaService.Domain.Agregados
{
    public class PuntoEntrega :Entity
    {
        
        public int Secuencia { get; private set; }
        public EstadoPuntoEntrega EstadoPunto { get; private set; }
        public Guid PaqueteId { get; private set; }


        public Guid RutaDistribucionId { get; private set; }

        private PuntoEntrega() { }

        
        public PuntoEntrega(Guid id, Guid paqueteId, int secuenciaInicial) : base(id)
        {
            if (paqueteId == Guid.Empty)
                throw new ArgumentException("PaqueteId no puede ser vacío.", nameof(paqueteId));

            PaqueteId = paqueteId;
            Secuencia = secuenciaInicial;
            EstadoPunto = EstadoPuntoEntrega.Pendiente;
        }

        
        internal void ReasignarSecuencia(int nuevaSecuencia)
        {
            if (nuevaSecuencia <= 0)
                throw new ArgumentException("La secuencia debe ser positiva.");

            Secuencia = nuevaSecuencia;
        }

        
        public void MarcarComoEntregado()
        {
            if (EstadoPunto != EstadoPuntoEntrega.Pendiente && EstadoPunto != EstadoPuntoEntrega.EnRuta)
                throw new InvalidOperationException($"No se puede marcar como entregado un punto en estado {EstadoPunto}.");

            EstadoPunto = EstadoPuntoEntrega.Entregado;
            
        }
    }
}
