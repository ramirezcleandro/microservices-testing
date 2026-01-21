using Joseco.DDD.Core.Results;
using MediatR;
using Logistica.Application.RutaDistribucion.Queries.Common;
using System.Collections.Generic;

namespace Logistica.Application.RutaDistribucion.Queries.ListarRutas
{
    public record ListarRutasQuery()
        : IRequest<Result<IReadOnlyList<RutaResumenDto>>>;
}
