using DomainLayer.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PriceService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly ILogger<PriceController> _logger;
        private readonly IMediator _mediator;

        public PriceController(ILogger<PriceController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GetCarPriceAsync([FromBody] PriceQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = new CalculateCarPriceMessage(query.Code, query.Start, query.End);
            var result = await _mediator.Send(message);

            return result.Handle<IActionResult>(
                success =>
                {
                    return Ok(success);
                },
                error =>
                {
                    return BadRequest(error);
                }
            );
        }
    }

    public record PriceQuery([Required] string Code, [Required] DateTime Start, [Required] DateTime End);
}