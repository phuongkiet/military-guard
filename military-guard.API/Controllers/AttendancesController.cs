using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.Attendances.Commands.CheckIn;

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
