using Joseco.DDD.Core.Results;
using Logistica.Application.RutaDistribucion.Queries.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.RutaDistribucion.Queries.GetProgresoRuta
{
    public class GetProgresoRutaHandler
       : IRequestHandler<GetProgresoRutaQuery, Result<ProgresoRutaDto>>
    {
        private readonly IRutaReadStore _read;
        public GetProgresoRutaHandler(IRutaReadStore read) => _read = read;

        public async Task<Result<ProgresoRutaDto>> Handle(GetProgresoRutaQuery request, CancellationToken ct)
        {
            var dto = await _read.GetProgresoRutaAsync(request.RutaId, ct);
            return dto is null
                ? Result.Failure<ProgresoRutaDto>(Error.NotFound("Ruta.NoEncontrada", $"Ruta {request.RutaId} no existe."))
                : Result.Success(dto);
        }
    }
}
