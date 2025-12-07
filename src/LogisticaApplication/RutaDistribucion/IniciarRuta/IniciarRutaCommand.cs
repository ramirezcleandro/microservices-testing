using Joseco.DDD.Core.Results;
using MediatR;

namespace Logistica.Application.RutaDistribucion.IniciarRuta
{
    public record IniciarRutaCommand(Guid RutaId) : IRequest<Result<Guid>>;
}
