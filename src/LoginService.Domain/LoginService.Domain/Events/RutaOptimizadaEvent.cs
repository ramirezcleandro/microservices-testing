using Joseco.DDD.Core.Abstractions;
using LogisticaService.Domain.Agregados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.Events
{
    public sealed record RutaOptimizadaEvent : DomainEvent
    {
        public Guid RutaId { get; init; }
        public Guid PersonalEntregaId { get; init; }

        // Se envía el nuevo orden completo de la ruta
        public IReadOnlyList<PuntoEntrega> PuntosOptimizados { get; init; }

        public RutaOptimizadaEvent(Guid rutaId, Guid personalEntregaId, IReadOnlyList<PuntoEntrega> puntosOptimizados)
        {
            RutaId = rutaId;
            PersonalEntregaId = personalEntregaId;
            PuntosOptimizados = puntosOptimizados;
        }

        private RutaOptimizadaEvent() { }
    }
}
