using Joseco.DDD.Core.Abstractions;
using LogisticaService.Domain.Events;
using LogisticaService.Domain.Repositories;
using MediatR;

namespace Logistica.Application.RutaDistribucion.EventHandlers
{
    public class EntregaConfirmadaEventHandler : INotificationHandler<EntregaConfirmadaEvent>
    {
        private readonly IRutaDistribucionRepository _repo;
        private readonly IUnitOfWork _uow;

        public EntregaConfirmadaEventHandler(IRutaDistribucionRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task Handle(EntregaConfirmadaEvent notification, CancellationToken ct)
        {
            // Busca la ruta EN CURSO que contenga ese PaqueteId (Guid)
            var ruta = await _repo.GetEnCursoByPaqueteIdAsync(notification.PaqueteId);
            if (ruta is null) return;

            try
            {
                ruta.MarcarPuntoEntregadoPorPaquete(notification.PaqueteId);
                ruta.CompletarSiTodosEntregados();
                await _uow.CommitAsync(ct);
            }
            catch
            {
                // log (no relanzar para no romper el publish)
            }
        }
    }
}
