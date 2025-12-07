using Joseco.DDD.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.Events
{
    
    public sealed record EntregaConfirmadaEvent : DomainEvent
    {
        
        public Guid PaqueteId { get; init; }
        public string EtiquetaId { get; init; }
        public DateTime TimestampConfirmacion { get; init; }

       
        public EntregaConfirmadaEvent(Guid paqueteId, string etiquetaId, DateTime timestampConfirmacion)
        {
            
            PaqueteId = paqueteId;
            EtiquetaId = etiquetaId;
            TimestampConfirmacion = timestampConfirmacion;
        }

        
        private EntregaConfirmadaEvent() { }
    }
}
