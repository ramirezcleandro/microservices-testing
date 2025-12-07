using Joseco.DDD.Core.Results;
using LogisticaService.Domain.Agregados;
using LogisticaService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.ServiciosDominio
{
    public static class VerificadorEntregaService
    {
      
        public static void AuditarPruebaGeografica(PaqueteEntrega paquete, IPoliticaTolerancia politicaValidacion,
            double distanciaEnMetros)
        {

            politicaValidacion.Validar(distanciaEnMetros);

        }

    }
}
