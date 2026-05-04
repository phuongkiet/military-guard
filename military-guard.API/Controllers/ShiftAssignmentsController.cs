using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.ShiftAssignments.Commands.CreateShiftAssignment;
using military_guard.Application.Features.ShiftAssignments.Commands.DeleteShiftAssignment;
using military_guard.Application.Features.ShiftAssignments.Commands.SubstituteMilitia;
using military_guard.Application.Features.ShiftAssignments.Commands.UpdateShiftAssignment;
using military_guard.Application.Features.ShiftAssignments.Queries.GetAllShiftAssignments;
using military_guard.Application.Features.ShiftAssignments.Queries.GetAvailableSubstitutes;
using military_guard.Application.Features.ShiftAssignments.Queries.GetShiftAssignmentById;

namespace military_guard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftAssignmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ShiftAssignmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        ////[HttpPost]
        ////public async Task<IActionResult> CreateShiftAssignment([FromBody] CreateShiftAssignmentCommand command, CancellationToken cancellationToken)
        ////{
        ////    var shiftAssignmentId = await _mediator.Send(command, cancellationToken);
        ////    return Ok(new { Message = "Sắp xếp ca thành công.", ShiftAssignmentId = shiftAssignmentId });
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllShiftAssignments([FromQuery] GetAllShiftAssignmentsQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetShiftAssignmentByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result); 
        }

        [HttpGet("{absentAssignmentId}/available-substitutes")]
        public async Task<IActionResult> GetAvailableSubstitutes(Guid absentAssignmentId)
        {
            var query = new GetAvailableSubstitutesQuery { AbsentAssignmentId = absentAssignmentId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShiftAssignmentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        [HttpPost("substitute")]
        public async Task<IActionResult> SubstituteMilitia([FromBody] SubstituteMilitiaCommand command)
        {
            var newAssignmentId = await _mediator.Send(command);
            return Ok(new { Message = "Điều động thành công", AssignmentId = newAssignmentId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShiftAssignmentCommand command)
        {
            command.Id = id;

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteShiftAssignmentCommand(id);
            await _mediator.Send(command);
            return NoContent(); 
        }
    }
}
