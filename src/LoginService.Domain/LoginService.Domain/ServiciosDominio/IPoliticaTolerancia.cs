using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.ServiciosDominio
{
    public interface IPoliticaTolerancia
    {
        void Validar(double distanciaEnMetros);
    }
}
