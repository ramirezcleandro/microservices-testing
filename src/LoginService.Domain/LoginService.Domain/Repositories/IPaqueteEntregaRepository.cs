using Joseco.DDD.Core.Abstractions;
using LogisticaService.Domain.Agregados;

namespace LogisticaService.Domain.Repositories
{
    public interface IPaqueteEntregaRepository : IRepository<PaqueteEntrega>
    {
        Task<PaqueteEntrega?> GetByEtiquetaIdAsync(string etiquetaId);
    }
}
