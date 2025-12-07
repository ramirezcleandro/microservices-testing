using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.Events
{
    public enum EstadoPuntoEntrega
    {
        Pendiente,
        EnRuta,
        Entregado,
        Fallido
    }
}
