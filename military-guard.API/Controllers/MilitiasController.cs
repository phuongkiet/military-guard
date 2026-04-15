
using MediatR;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.Employees.Commands.DeleteMilitias;
using military_guard.Application.Features.Militias.Commands.CreateMilitias;
using military_guard.Application.Features.Militias.Commands.UpdateMilitias;
using military_guard.Application.Features.Militias.Queries.GetMilitiaById;
using military_guard.Application.Features.Militias.Queries.GetMilitias;
using military_guard.Domain.Entities;

namespace military_guard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilitiasController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Chỉ tiêm (inject) đúng 1 cái IMediator vào đây
        public MilitiasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMilitias([FromQuery] GetAllMilitiasQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMilitiaById(Guid id)
        {
            var query = new GetMilitiaByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMilitia([FromBody] CreateMilitiaCommand command)
        {
            var militiaId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetMilitiaById), new { id = militiaId }, militiaId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMilitia(Guid id, [FromBody] UpdateMilitiaCommand command)
        {
            command.Id = id; 
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMilitia(Guid id)
        {
            await _mediator.Send(new DeleteMilitiaCommand(id));
            return NoContent();
        }
    }
}
