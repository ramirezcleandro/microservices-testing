using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using Logistica.Infrastructure.Interfaces;
using LogisticaService.Domain.Repositories;
using LogisticaService.Domain.ServiciosDominio;
using LogisticaService.Domain.ValueObjects;
using MediatR;



namespace Logistica.Application.PaqueteEntrega.ConfirmarEntrega
{
    public record ConfirmarEntregaHandler : IRequestHandler<ConfirmarEntregaCommand, Result<Guid>>
    {

        
        private readonly IPaqueteEntregaRepository _paqueteRepository;
        private readonly IGeolocationService _geolocationService; 
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IPoliticaTolerancia _toleranciaPolicy; 

        public ConfirmarEntregaHandler(
            IPaqueteEntregaRepository paqueteRepository,
            IGeolocationService geolocationService,
            IUnitOfWork unitOfWork,
            IPoliticaTolerancia toleranciaPolicy)
        {
            _paqueteRepository = paqueteRepository;
            _geolocationService = geolocationService;
            _unitOfWork = unitOfWork;
            _toleranciaPolicy = toleranciaPolicy;
        }
        public async Task<Result<Guid>> Handle(ConfirmarEntregaCommand request, CancellationToken cancellationToken)
        {
            
            var paquete = await _paqueteRepository.GetByIdAsync(request.PaqueteId);

            
            if (paquete is null)
            {
                var error = Error.NotFound(
                    "Paquete.NotFound",
                    "Paquete con ID {0} no fue encontrado.",
                    request.PaqueteId.ToString()
                );
                
                return Result.Failure<Guid>(error);
            }

            try
            {
                
                var geopointConfirmacion = new DireccionGeolocalizada(
                    paquete.DireccionGeolocalizada.DireccionCompleta,
                    request.LatitudConfirmacion,
                    request.LongitudConfirmacion
                );

                var distanciaEnMetros = await _geolocationService.CalculateDistanceMetersAsync(
                    paquete.DireccionGeolocalizada, 
                    geopointConfirmacion           
                );

             
                var registroEntrega = new RegistroEntrega(
                     request.TipoPrueba,
                     DateTime.UtcNow, 
                     geopointConfirmacion,
                     request.UrlEvidencia
                 );


                paquete.ConfirmarEntrega(
                    registroEntrega,
                    _toleranciaPolicy ,
                    distanciaEnMetros
                );

 
                await _unitOfWork.CommitAsync();

       
                return Result.Success(paquete.Id);

            }
            
            catch (InvalidOperationException ex)
            {
                
                var invalidOpError = Error.Failure(
                    "Paquete.InvalidOperation",
                    "Validación de Dominio fallida: {0}",
                    ex.Message
                );
                return Result.Failure<Guid>(invalidOpError);
            }
            
            catch (Exception ex)
            {
                var generalError = Error.Problem(
                    "General.Unhandled",
                    "Ocurrió un error inesperado al procesar la entrega: {0}",
                    ex.Message
                );
                return Result.Failure<Guid>(generalError);
            }
        }
    }
}
