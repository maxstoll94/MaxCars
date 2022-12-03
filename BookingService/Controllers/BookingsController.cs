using DomainLayer.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PriceService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger<BookingsController> _logger;
        private readonly IMediator _mediator;

        public BookingsController(ILogger<BookingsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = new BookCarMessage(request.Code, request.Start, request.End);
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

    public record BookingRequest([Required] string Code, [Required] DateTime Start, [Required] DateTime End);
}