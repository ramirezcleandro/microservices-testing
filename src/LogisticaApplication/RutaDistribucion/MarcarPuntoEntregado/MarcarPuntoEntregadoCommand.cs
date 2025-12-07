using Joseco.DDD.Core.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Application.RutaDistribucion.MarcarPuntoEntregado
{
    public record MarcarPuntoEntregadoCommand(Guid RutaId, Guid PaqueteId) : IRequest<Result<Guid>>;
}
