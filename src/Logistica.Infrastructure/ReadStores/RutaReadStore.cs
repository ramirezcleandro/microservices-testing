using Logistica.Application.RutaDistribucion.Queries;
using Logistica.Application.RutaDistribucion.Queries.Common;
using Logistica.Infrastructure.Persistence.DomainModel;
using LogisticaService.Domain.Events;
using Microsoft.EntityFrameworkCore;


namespace Logistica.Infrastructure.ReadStores
{
    internal class RutaReadStore : IRutaReadStore
    {
        private readonly DomainDbContext _db;
        public RutaReadStore(DomainDbContext db) => _db = db;

        public async Task<DetalleRutaDto?> GetDetalleRutaAsync(Guid rutaId, CancellationToken ct = default)
        {
            return await _db.RutasDistribucion
                .AsNoTracking()
                .Where(r => r.Id == rutaId)
                .Select(r => new DetalleRutaDto(
                    r.Id,
                    r.EstadoRuta.ToString(),
                    r.Fecha,
                    r.PersonalEntregaId,
                    r.AlmacenUbicacion.DireccionCompleta,
                    r.AlmacenUbicacion.Latitud,
                    r.AlmacenUbicacion.Longitud,
                    r.Puntos
                        .OrderBy(p => p.Secuencia)
                        .Select(p => new PuntoEntregaDto(
                            p.Id,
                            p.PaqueteId,
                            p.Secuencia,
                            p.EstadoPunto.ToString()
                        ))
                        .ToList()
                ))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ProgresoRutaDto?> GetProgresoRutaAsync(Guid rutaId, CancellationToken ct = default)
        {
            // Proyección directa a DTO
            var dto = await _db.RutasDistribucion
                .AsNoTracking()
                .Where(r => r.Id == rutaId)
                .Select(r => new
                {
                    r.Id,
                    r.EstadoRuta,
                    r.Fecha,
                    r.PersonalEntregaId,
                    Puntos = r.Puntos.Select(p => new
                    {
                        p.Id,
                        p.PaqueteId,
                        p.Secuencia,
                        Estado = p.EstadoPunto
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (dto is null) return null;

            var total = dto.Puntos.Count;
            var entregados = dto.Puntos.Count(p => p.Estado == EstadoPuntoEntrega.Entregado);
            var pendientes = total - entregados;
            var porcentaje = total == 0 ? 0.0 : Math.Round((double)entregados * 100.0 / total, 1);

            int? siguiente = dto.Puntos
                .Where(p => p.Estado != EstadoPuntoEntrega.Entregado)
                .OrderBy(p => p.Secuencia)
                .Select(p => (int?)p.Secuencia)
                .FirstOrDefault();

            return new ProgresoRutaDto(
                dto.Id,
                dto.EstadoRuta.ToString(),
                dto.Fecha,
                dto.PersonalEntregaId,
                total,
                entregados,
                pendientes,
                porcentaje,
                siguiente,
                dto.Puntos
                    .OrderBy(p => p.Secuencia)
                    .Select(p => new PuntoEstadoDto(
                        p.Id,
                        p.PaqueteId,
                        p.Secuencia,
                        p.Estado.ToString()
                    ))
                    .ToList()
            );
        }
    }
}
