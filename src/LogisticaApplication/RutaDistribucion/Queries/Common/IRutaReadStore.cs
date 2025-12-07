using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.RutaDistribucion.Queries.Common
{
    public interface IRutaReadStore
    {
        Task<DetalleRutaDto?> GetDetalleRutaAsync(Guid rutaId, CancellationToken ct);
        Task<ProgresoRutaDto?> GetProgresoRutaAsync(Guid rutaId, CancellationToken ct);
        //Task<List<RutaItemDto>> GetRutasPorPersonalYEstadoAsync(Guid personalId, string estado, CancellationToken ct);
    }
}
