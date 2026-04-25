using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.Attendances.Commands.CheckIn;
using military_guard.Application.Features.Attendances.Queries.GetLiveAttendances;

namespace military_guard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendancesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AttendancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("shift/{shiftId}")]
        public async Task<IActionResult> GetLiveAttendancesByShift(Guid shiftId, [FromQuery] DateOnly? date)
        {
            var query = new GetLiveAttendancesQuery
            {
                ShiftId = shiftId,
                Date = date
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new
            {
                Message = "Điểm danh thành công",
                Data = response
            });
        }
    }
}
