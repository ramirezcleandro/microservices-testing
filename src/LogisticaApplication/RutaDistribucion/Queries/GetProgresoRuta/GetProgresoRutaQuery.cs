using Joseco.DDD.Core.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.RutaDistribucion.Queries.GetProgresoRuta
{
    public record GetProgresoRutaQuery(Guid RutaId) : IRequest<Result<ProgresoRutaDto>>;
}
