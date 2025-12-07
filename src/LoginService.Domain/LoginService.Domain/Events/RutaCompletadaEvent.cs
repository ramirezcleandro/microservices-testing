using Joseco.DDD.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.Events
{
    public sealed record RutaCompletadaEvent : DomainEvent
    {
        public Guid RutaId { get; init; }
        public Guid PersonalEntregaId { get; init; }

        public RutaCompletadaEvent(Guid rutaId, Guid personalEntregaId)
        {
            RutaId = rutaId;
            PersonalEntregaId = personalEntregaId;
        }

        private RutaCompletadaEvent() { }
    }
}
