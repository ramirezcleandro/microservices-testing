using Joseco.DDD.Core.Results;
using Logistica.Application.RutaDistribucion.Queries.Common;
using MediatR;

namespace Logistica.Application.RutaDistribucion.Queries.GetDetalleRuta
{
    public record GetDetalleRutaQuery(Guid RutaId) : IRequest<Result<DetalleRutaDto>>;

  
}
