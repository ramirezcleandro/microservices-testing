using Logistica.Application.RutaDistribucion.AgregarPaqueteARuta;
using Logistica.Application.RutaDistribucion.CrearRuta;
using Logistica.Application.RutaDistribucion.IniciarRuta;
using Logistica.Application.RutaDistribucion.MarcarPuntoEntregado;
using Logistica.Application.RutaDistribucion.OptimizarRuta;
using Logistica.Application.RutaDistribucion.Queries.GetProgresoRuta;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logistica.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RutaDistribucionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RutaDistribucionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearRuta([FromBody] CrearRutaCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                var errorResponse = new
                {
                    Error = result.Error.Code,
                    Mensaje = result.Error.Description
                };
                return BadRequest(errorResponse);
            }

            return Ok(result.Value);
        }

        [HttpPost("{rutaId:guid}/iniciar")]
        public async Task<IActionResult> Iniciar(Guid rutaId)
        {
            var r = await _mediator.Send(new IniciarRutaCommand(rutaId));
            return r.IsFailure ? BadRequest(new { error = r.Error.Code, mensaje = r.Error.Description }) : Ok(r.Value);
        }


        [HttpPost("{rutaId}/agregar-paquete")]
        public async Task<IActionResult> AgregarPaquete(
            Guid rutaId,
            [FromBody] AgregarPaqueteARutaCommand command)
        {
            // Sobrescribimos la rutaId del URL al comando
            var cmd = command with { RutaId = rutaId };

            var result = await _mediator.Send(cmd);

            if (result.IsFailure)
            {
                var errorResponse = new
                {
                    Error = result.Error.Code,
                    Mensaje = result.Error.Description
                };
                return BadRequest(errorResponse);
            }

            return Ok(result.Value);
        }

        [HttpPost("{rutaId:guid}/puntos/{paqueteId:guid}/entregado")]
        public async Task<IActionResult> MarcarEntregado(Guid rutaId, Guid paqueteId)
        {
            var r = await _mediator.Send(new MarcarPuntoEntregadoCommand(rutaId, paqueteId));
            return r.IsFailure ? BadRequest(new { error = r.Error.Code, mensaje = r.Error.Description }) : Ok(r.Value);
        }


        [HttpPost("{rutaId}/optimizar")]
        public async Task<IActionResult> OptimizarRuta(
            Guid rutaId,
            [FromBody] OptimizarRutaCommand command)
        {
            var cmd = command with { RutaId = rutaId };

            var result = await _mediator.Send(cmd);

            if (result.IsFailure)
            {
                var errorResponse = new
                {
                    Error = result.Error.Code,
                    Mensaje = result.Error.Description
                };
                return BadRequest(errorResponse);
            }

            return Ok(result.Value);
        }

        // src/Logistica.WebApi/Controllers/RutaDistribucionController.cs
        [HttpGet("{rutaId:guid}/progreso")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProgreso(Guid rutaId)
        {
            var result = await _mediator.Send(new GetProgresoRutaQuery(rutaId));
            if (result.IsFailure)
            {
                return NotFound(new { Error = result.Error.Code, Mensaje = result.Error.Description });
            }
            return Ok(result.Value);
        }
    }
}
