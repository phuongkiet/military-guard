using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.Accounts.Commands.BanAccount;
using military_guard.Application.Features.Accounts.Commands.UnbanAccount;
using military_guard.Application.Features.Accounts.Queries.GetAllAccounts;

namespace military_guard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts([FromQuery]GetAllAccountsQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}/ban")]
        public async Task<IActionResult> BanAccount(Guid id, CancellationToken cancellationToken)
        {
            var command = new BanAccountCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result)
            {
                return BadRequest(new { Message = "Không tìm thấy tài khoản hoặc có lỗi xảy ra." });
            }

            return Ok(new { Message = "Đã khóa tài khoản thành công." });
        }

        [HttpPut("{id}/unban")]
        public async Task<IActionResult> UnbanAccount(Guid id, CancellationToken cancellationToken)
        {
            var command = new UnbanAccountCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result)
            {
                return BadRequest(new { Message = "Không tìm thấy tài khoản hoặc có lỗi xảy ra." });
            }

            return Ok(new { Message = "Đã hủy khóa tài khoản thành công." });
        }
    }
}
