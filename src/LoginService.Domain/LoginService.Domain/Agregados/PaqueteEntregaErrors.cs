using Joseco.DDD.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.Agregados
{
    internal static class PaqueteEntregaErrors
    {
        public static Error EntregaYaConfirmada => Error.Conflict(
            "Paquete.EntregaYaConfirmada",
            "El paquete ya ha sido marcado como entregado y no se puede modificar su registro."
        );


      
        public static Error RegistroEntregaNulo => Error.Failure(
            "Paquete.RegistroEntregaNulo",
            "El objeto de registro de entrega no puede ser nulo al confirmar la entrega."
        );

        
        public static Error EtiquetaIdVacia => Error.Failure(
            "Paquete.EtiquetaIdVacia",
            "El ID de la etiqueta del paquete no debe ser un GUID vacío."
        );

        public static Error DistanciaExcedida(double distanciaReal, double distanciaMaxima) => Error.Conflict(
            "Paquete.DistanciaExcedida",
            "La prueba GPS del repartidor está a {0} metros del destino planificado. El límite máximo es {1} metros.",
            distanciaReal.ToString("F2"),
            distanciaMaxima.ToString("F2")
        );
    }
}
