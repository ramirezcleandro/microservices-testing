using Joseco.DDD.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Domain.Events
{
    public sealed record PaqueteAgregadoARutaEvent : DomainEvent
    {
        public Guid RutaId { get; init; }
        public Guid PaqueteId { get; init; }

        public PaqueteAgregadoARutaEvent(Guid rutaId, Guid paqueteId)
        {
            RutaId = rutaId;
            PaqueteId = paqueteId;
        }

        // Constructor privado para EF y serialización
        private PaqueteAgregadoARutaEvent() { }
    }
}
