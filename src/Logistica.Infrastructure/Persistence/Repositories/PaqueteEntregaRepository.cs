using Logistica.Infrastructure.Persistence.DomainModel;
using LogisticaService.Domain.Agregados;
using LogisticaService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Logistica.Infrastructure.Persistence.Repositories
{

    
    internal class PaqueteEntregaRepository : IPaqueteEntregaRepository
    {
        private readonly DomainDbContext _dbContext;

        public PaqueteEntregaRepository(DomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        

        public async Task<PaqueteEntrega?> GetByIdAsync(Guid id, bool readOnly = false)
        {

            IQueryable<PaqueteEntrega> query = _dbContext.Set<PaqueteEntrega>();

            if (readOnly)
            {
                query = query.AsNoTracking();
            }
     
            var paquete = await query.FirstOrDefaultAsync(p => p.Id == id); // p.Id.ToString() == id.ToString() debe eliminarse

            return paquete;
        }

        public async Task AddAsync(PaqueteEntrega entity)
        {
           
            await _dbContext.Paquetes.AddAsync(entity);
        }

        public Task UpdateAsync(PaqueteEntrega item)
        {
            _dbContext.Paquetes.Update(item);
            return Task.CompletedTask;
        }


        public async Task<PaqueteEntrega?> GetByEtiquetaIdAsync(string etiquetaId)
        {
   
            return await _dbContext.Paquetes
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.EtiquetaId == etiquetaId);
        }


        public async Task DeleteAsync(Guid id)
        {
        
            var obj = await GetByIdAsync(id);

            if (obj is not null)
            {
                _dbContext.Paquetes.Remove(obj);
            }
        }
    }
}
