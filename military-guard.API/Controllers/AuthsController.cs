using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.Auths.Commands.Login;
using military_guard.Application.Features.Auths.Commands.SignUp;

namespace military_guard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous] 
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response); // HTTP 200
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] SignUpCommand command, CancellationToken cancellationToken)
        {
            var accountId = await _mediator.Send(command, cancellationToken);

            return Ok(new
            {
                Message = "Tạo tài khoản thành công.",
                AccountId = accountId
            });
        }
    }
}
