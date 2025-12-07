using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using LogisticaService.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.RutaDistribucion.OptimizarRuta
{
    public class OptimizarRutaHandler : IRequestHandler<OptimizarRutaCommand, Result<Guid>>
    {
        private readonly IRutaDistribucionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public OptimizarRutaHandler(
            IRutaDistribucionRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(OptimizarRutaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var ruta = await _repository.GetByIdAsync(request.RutaId);
                if (ruta is null)
                    return Result.Failure<Guid>(Error.NotFound("Ruta.NoEncontrada", $"Ruta {request.RutaId} no existe."));

                
                ruta.OptimizarRuta(request.NuevoOrden);

                
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(ruta.Id);
            }
            catch (Exception ex)
            {
                var error = Error.Problem("Ruta.OptimizarError", "Error al optimizar la ruta: {0}", ex.Message);
                return Result.Failure<Guid>(error);
            }
        }
    }
}
