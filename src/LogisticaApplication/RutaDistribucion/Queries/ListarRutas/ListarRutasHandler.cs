using Joseco.DDD.Core.Results;
using MediatR;
using Logistica.Application.RutaDistribucion.Queries.Common;

namespace Logistica.Application.RutaDistribucion.Queries.ListarRutas
{
    public class ListarRutasHandler
        : IRequestHandler<ListarRutasQuery, Result<IReadOnlyList<RutaResumenDto>>>
    {
        private readonly IRutaReadStore _read;

        public ListarRutasHandler(IRutaReadStore read)
        {
            _read = read;
        }

        public async Task<Result<IReadOnlyList<RutaResumenDto>>> Handle(
            ListarRutasQuery request,
            CancellationToken ct)
        {
            var rutas = await _read.ListarRutasAsync(ct);

            // A diferencia de GetDetalle, aquí NO es error no tener datos
            return Result.Success<IReadOnlyList<RutaResumenDto>>(
                rutas ?? Array.Empty<RutaResumenDto>()
            );
        }
    }
}
