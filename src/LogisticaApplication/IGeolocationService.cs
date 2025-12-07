using LogisticaService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Infrastructure.Interfaces
{
    public interface IGeolocationService
    {
        Task<double> CalculateDistanceMetersAsync(
            DireccionGeolocalizada origen,
            DireccionGeolocalizada destino);
    }
}
