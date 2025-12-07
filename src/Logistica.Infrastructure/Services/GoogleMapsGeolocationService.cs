using Logistica.Infrastructure.Interfaces;
using LogisticaService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Infrastructure.Services
{
    public class GoogleMapsGeolocationService : IGeolocationService
    {
        public Task<double> CalculateDistanceMetersAsync(
            DireccionGeolocalizada origen,
            DireccionGeolocalizada destino)
        {
            
            return Task.FromResult(15.5); 
        }
    }
}
