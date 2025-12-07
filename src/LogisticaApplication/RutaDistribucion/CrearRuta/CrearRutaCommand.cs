using Joseco.DDD.Core.Results;
using MediatR;


namespace Logistica.Application.RutaDistribucion.CrearRuta
{
    public record CrearRutaCommand(
        DateOnly Fecha,
        Guid PersonalEntregaId,
        string DireccionAlmacen,
        double Latitud,
        double Longitud
    ) : IRequest<Result<Guid>>;
}
