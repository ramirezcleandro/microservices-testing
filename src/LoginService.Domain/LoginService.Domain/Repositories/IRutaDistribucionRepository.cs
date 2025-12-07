using Joseco.DDD.Core.Abstractions;
using LogisticaService.Domain.Agregados;
using LogisticaService.Domain.Enums;

namespace LogisticaService.Domain.Repositories
{
    public interface IRutaDistribucionRepository : IRepository<RutaDistribucion>
    {
        Task<RutaDistribucion?> GetRutaActivaByPersonalIdAsync(Guid personalId, EstadoRuta estadoRuta);
        Task AddPuntoAsync(PuntoEntrega punto);

        Task<RutaDistribucion?> GetEnCursoByPaqueteIdAsync(Guid paqueteId);

    }
}
