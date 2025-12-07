using Joseco.DDD.Core.Results;
using Logistica.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.PaqueteEntrega.ConfirmarEntrega
{
    public  record ConfirmarEntregaCommand(
        Guid PaqueteId,
        double LatitudConfirmacion,
        double LongitudConfirmacion,
        TipoPruebaEntrega TipoPrueba,
        string UrlEvidencia
    ) : IRequest<Result<Guid>>;
}
