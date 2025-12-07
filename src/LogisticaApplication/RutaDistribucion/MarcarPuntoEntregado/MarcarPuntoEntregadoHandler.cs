using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using LogisticaService.Domain.Repositories;
using MediatR;

namespace Logistica.Application.RutaDistribucion.MarcarPuntoEntregado
{
    public class MarcarPuntoEntregadoHandler : IRequestHandler<MarcarPuntoEntregadoCommand, Result<Guid>>
    {
        private readonly IRutaDistribucionRepository _repo;
        private readonly IUnitOfWork _uow;

        public MarcarPuntoEntregadoHandler(IRutaDistribucionRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<Result<Guid>> Handle(MarcarPuntoEntregadoCommand request, CancellationToken ct)
        {
            var ruta = await _repo.GetByIdAsync(request.RutaId);
            if (ruta is null)
                return Result.Failure<Guid>(Error.NotFound("Ruta.NoEncontrada", $"Ruta {request.RutaId} no existe."));

            try
            {
                ruta.MarcarPuntoEntregadoPorPaquete(request.PaqueteId);
                ruta.CompletarSiTodosEntregados();
                await _uow.CommitAsync(ct);
                return Result.Success(ruta.Id);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(Error.Problem("Ruta.EntregaPuntoError", "No se pudo marcar entregado: {0}", ex.Message));
            }
        }
    }
}
