using Joseco.DDD.Core.Abstractions;
using Logistica.Domain.Events;
using LogisticaService.Domain.Events;
using LogisticaService.Domain.Repositories;
using MediatR;

namespace Logistica.Application.RutaDistribucion.EventHandlers
{
    /// <summary>
    /// Manejador de eventos que reacciona cuando se agrega un paquete a una ruta.
    /// </summary>
    public class PaqueteAgregadoARutaEventHandler : INotificationHandler<PaqueteAgregadoARutaEvent>
    {
        private readonly IRutaDistribucionRepository _rutaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaqueteAgregadoARutaEventHandler(
            IRutaDistribucionRepository rutaRepository,
            IUnitOfWork unitOfWork)
        {
            _rutaRepository = rutaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(PaqueteAgregadoARutaEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"📦 Evento recibido → Ruta {notification.RutaId}, Paquete {notification.PaqueteId}");

            var ruta = await _rutaRepository.GetByIdAsync(notification.RutaId);
            if (ruta == null)
            {
                Console.WriteLine($"⚠️ Ruta {notification.RutaId} no encontrada al manejar evento.");
                return;
            }

            // Lógica adicional opcional, por ejemplo:
            // enviar notificación, registrar log, etc.
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
