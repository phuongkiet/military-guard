using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.DutyShifts.Commands.CreateDutyShift;
using military_guard.Application.Features.DutyShifts.Commands.DeleteDutyShift;
using military_guard.Application.Features.DutyShifts.Commands.UpdateDutyShift;
using military_guard.Application.Features.DutyShifts.Queries.GetAllDutyShifts;
using military_guard.Application.Features.DutyShifts.Queries.GetDutyShiftById;

namespace military_guard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DutyShiftsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DutyShiftsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDutyShiftsQuery query, CancellationToken cancellationToken)
        {
            var dutyShifts = await _mediator.Send(query, cancellationToken);
            return Ok(dutyShifts);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetDutyShiftById([FromQuery] GetDutyShiftByIdQuery query, CancellationToken cancellationToken)
        {
            var dutyShift = await _mediator.Send(query, cancellationToken);
            return Ok(dutyShift);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDutyShiftCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetDutyShiftById), new { id = result }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDutyShiftCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;

            await _mediator.Send(command, cancellationToken);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteDutyShiftCommand(id);
            await _mediator.Send(command, cancellationToken);
            return NoContent(); 
        }
    }
}
