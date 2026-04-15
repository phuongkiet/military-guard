using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.GuardPosts.Commands.CreateGuardPost;
using military_guard.Application.Features.GuardPosts.Commands.DeleteGuardPost;
using military_guard.Application.Features.GuardPosts.Commands.UpdateGuardPost;
using military_guard.Application.Features.GuardPosts.Queries.GetAllGuardPosts;
using military_guard.Application.Features.GuardPosts.Queries.GetGuardPostById;

namespace military_guard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardPostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GuardPostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGuardPost([FromQuery] GetAllGuardPostsQuery query, CancellationToken cancellationToken)
        {
            var guardPost = await _mediator.Send(query, cancellationToken);
            return Ok(guardPost);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetGuardPostById([FromQuery] GetGuardPostByIdQuery query, CancellationToken cancellationToken)
        {
            var guardPost = await _mediator.Send(query, cancellationToken);
            return Ok(guardPost);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGuardPostCommand command, CancellationToken cancellationToken)
        {
            var guardPostId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetGuardPostById), new { id = guardPostId }, guardPostId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGuardPostCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;

            await _mediator.Send(command, cancellationToken);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteGuardPostCommand(id);
            await _mediator.Send(command, cancellationToken);
            return NoContent(); 
        }
    }
}
