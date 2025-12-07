using Logistica.Infrastructure.Persistence.DomainModel;
using LogisticaService.Domain.Agregados;
using LogisticaService.Domain.Enums;
using LogisticaService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Logistica.Infrastructure.Persistence.Repositories
{
    internal class RutaDistribucionRepository : IRutaDistribucionRepository
    {
        private readonly DomainDbContext _dbContext;

        public RutaDistribucionRepository(DomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Obtiene una ruta de distribución por su Id, incluyendo los puntos de entrega.
        /// </summary>
        public async Task<RutaDistribucion?> GetByIdAsync(Guid id, bool readOnly = false)
        {
            // ✅ Incluir correctamente la colección de puntos
            IQueryable<RutaDistribucion> query = _dbContext.RutasDistribucion
                .Include(r => r.Puntos);

            if (readOnly)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Agrega una nueva ruta de distribución.
        /// </summary>
        public async Task AddAsync(RutaDistribucion entity)
        {
            await _dbContext.RutasDistribucion.AddAsync(entity);
        }

        public Task AddPuntoAsync(PuntoEntrega punto)
        {
            _dbContext.Set<PuntoEntrega>().Add(punto); 
            return Task.CompletedTask;
        }

        /// <summary>
        /// Actualiza una ruta existente (por ejemplo, tras optimización o entrega).
        /// </summary>
        public Task UpdateAsync(RutaDistribucion entity)
        {
            _dbContext.RutasDistribucion.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Elimina una ruta por su Id.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var ruta = await GetByIdAsync(id);
            if (ruta is not null)
                _dbContext.RutasDistribucion.Remove(ruta);
        }

        /// <summary>
        /// Busca la ruta activa (por ejemplo, en estado 'Creada' o 'EnCurso') de un repartidor.
        /// </summary>
        public async Task<RutaDistribucion?> GetRutaActivaByPersonalIdAsync(Guid personalId, EstadoRuta estadoRuta)
        {
            return await _dbContext.RutasDistribucion
                .Include(r => r.Puntos)
                .FirstOrDefaultAsync(r =>
                    r.PersonalEntregaId == personalId &&
                    r.EstadoRuta == estadoRuta);
        }

        public async  Task<RutaDistribucion?> GetEnCursoByPaqueteIdAsync(Guid paqueteId)
        {
            return await _dbContext.RutasDistribucion
                .Include(r => r.Puntos)
                .Where(r => r.EstadoRuta == EstadoRuta.EnCurso &&
                            r.Puntos.Any(p => p.PaqueteId == paqueteId))
                .FirstOrDefaultAsync();
        }
    }
}
