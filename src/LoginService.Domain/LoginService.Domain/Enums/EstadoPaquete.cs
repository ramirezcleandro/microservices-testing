using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.Enums
{
    public enum EstadoPaquete
    {
        Recibido = 0,
        EnRuta = 1,
        Entregado = 2,
        FalloEntrega = 3
    }
}
