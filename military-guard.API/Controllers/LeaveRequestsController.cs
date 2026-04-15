using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using military_guard.Application.Features.LeaveRequests.Commands.ProcessLeaveRequest;
using military_guard.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;
using military_guard.Application.Features.LeaveRequests.Queries.GetPagedLeaveRequests;

namespace military_guard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedLeaveRequestsQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetPaged([FromQuery] GetLeaveRequestByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            var leaveRequestId = await _mediator.Send(command, cancellationToken);
            return StatusCode(201, new { Id = leaveRequestId, Message = "Nộp đơn thành công." });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ProcessLeaveRequest(Guid id, [FromBody] ProcessLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            command.LeaveRequestId = id; 

            await _mediator.Send(command, cancellationToken);
            return NoContent(); 
        }
    }
}
