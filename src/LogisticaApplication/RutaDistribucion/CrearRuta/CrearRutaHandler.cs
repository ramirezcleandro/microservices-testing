using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using LogisticaService.Domain.Repositories;
using LogisticaService.Domain.ValueObjects;
using MediatR;
using LogisticaService.Domain.Agregados;
namespace Logistica.Application.RutaDistribucion.CrearRuta
{
    public class CrearRutaHandler : IRequestHandler<CrearRutaCommand, Result<Guid>>
    {
        private readonly IRutaDistribucionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CrearRutaHandler(IRutaDistribucionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CrearRutaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var almacen = new DireccionGeolocalizada(
                    request.DireccionAlmacen,
                    request.Latitud,
                    request.Longitud
                );

                var ruta = new LogisticaService.Domain.Agregados.RutaDistribucion(
                    Guid.NewGuid(),
                    request.Fecha,
                    request.PersonalEntregaId,
                    almacen
                );

                await _repository.AddAsync(ruta);
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(ruta.Id);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "Sin detalles adicionales";
                var error = Error.Problem(
                    "Ruta.CreacionError",
                    $"Error al crear ruta: {ex.Message}. Inner: {inner}"
                );
                return Result.Failure<Guid>(error);
            }
        }
    }
}
