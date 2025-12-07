using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using LogisticaService.Domain.Repositories;
using MediatR;

namespace Logistica.Application.RutaDistribucion.AgregarPaqueteARuta
{
    public class AgregarPaqueteARutaHandler : IRequestHandler<AgregarPaqueteARutaCommand, Result<Guid>>
    {
        private readonly IRutaDistribucionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AgregarPaqueteARutaHandler(
            IRutaDistribucionRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AgregarPaqueteARutaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var ruta = await _repository.GetByIdAsync(request.RutaId);
                if (ruta is null)
                    return Result.Failure<Guid>(Error.NotFound("Ruta.NoEncontrada", $"Ruta {request.RutaId} no existe."));

                var punto = ruta.AgregarPaquete(request.PaqueteId);


                await _repository.AddPuntoAsync(punto);

                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(ruta.Id);
            }
            catch (Exception ex)
            {
                var error = Error.Problem("Ruta.AgregarPaqueteError", "Error al agregar paquete a la ruta: {0}", ex.Message);
                return Result.Failure<Guid>(error);
            }
        }
    }
}
