using Joseco.DDD.Core.Abstractions;
using LogisticaService.Domain.Repositories;
using LogisticaService.Domain.ServiciosDominio;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistica.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddSingleton<IPoliticaTolerancia, ToleranciaCeroPolitica>();

            return services;
            
        }
    }
}
