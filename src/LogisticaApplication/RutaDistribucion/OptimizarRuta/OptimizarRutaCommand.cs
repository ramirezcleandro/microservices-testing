using Joseco.DDD.Core.Results;
using MediatR;

namespace Logistica.Application.RutaDistribucion.OptimizarRuta
{
    public record OptimizarRutaCommand(
        Guid RutaId,
        Dictionary<Guid, int> NuevoOrden
    ) : IRequest<Result<Guid>>;
}
