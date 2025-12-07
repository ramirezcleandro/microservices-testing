using Joseco.DDD.Core.Results;
using Logistica.Application.RutaDistribucion.Queries.Common;
using MediatR;

namespace Logistica.Application.RutaDistribucion.Queries.GetDetalleRuta
{
    public class GetDetalleRutaHandler
      : IRequestHandler<GetDetalleRutaQuery, Result<DetalleRutaDto>>
    {
        private readonly IRutaReadStore _read;
        public GetDetalleRutaHandler(IRutaReadStore read) => _read = read;

        public async Task<Result<DetalleRutaDto>> Handle(GetDetalleRutaQuery request, CancellationToken ct)
        {
            var dto = await _read.GetDetalleRutaAsync(request.RutaId, ct);
            return dto is null
                ? Result.Failure<DetalleRutaDto>(Error.NotFound("Ruta.NoEncontrada", $"Ruta {request.RutaId} no existe."))
                : Result.Success(dto);
        }
    }
}
