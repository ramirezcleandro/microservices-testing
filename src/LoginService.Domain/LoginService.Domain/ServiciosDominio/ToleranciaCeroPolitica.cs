using Joseco.DDD.Core.Results;
using LogisticaService.Domain.Agregados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.ServiciosDominio
{
    public  class ToleranciaCeroPolitica :  IPoliticaTolerancia
    {
      

        private const double Tolerancia = 150.0; // ¡Tolerancia de 150 metros!

        public void Validar(double distanciaReportadaEnMetros)
        {
            if (distanciaReportadaEnMetros > Tolerancia)
            {
                var error = PaqueteEntregaErrors.DistanciaExcedida(distanciaReportadaEnMetros, Tolerancia);
                throw new DomainException(error);
            }
        }
    }
}
