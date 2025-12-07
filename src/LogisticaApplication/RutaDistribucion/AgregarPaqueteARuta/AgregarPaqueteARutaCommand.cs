using Joseco.DDD.Core.Results;
using MediatR;

namespace Logistica.Application.RutaDistribucion.AgregarPaqueteARuta
{
    public record AgregarPaqueteARutaCommand(
         Guid RutaId,
         Guid PaqueteId
     ) : IRequest<Result<Guid>>;
}
