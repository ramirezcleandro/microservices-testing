using Logistica.Application.PaqueteEntrega.ConfirmarEntrega;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logistica.WebApi.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class PaqueteEntregaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaqueteEntregaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("confirmar-entrega")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmarEntrega(
            [FromBody] ConfirmarEntregaCommand command)
        {

            var result = await _mediator.Send(command); // IMediator.Send()

            
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
     }
}
