using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.HolidayEvents.Commands.CreateHolidayEvent;
using military_guard.Application.Features.HolidayEvents.Queries.GetAllHolidayEvents;

namespace military_guard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayEventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HolidayEventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] GetAllHolidayEventsQuery query, CancellationToken cancellationToken)
        {
            var events = await _mediator.Send(query, cancellationToken);
            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHolidayEvent([FromBody] CreateHolidayEventCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new
            {
                Message = "Tạo ngày lễ thành công",
                Data = response
            });
        }
    }
}
