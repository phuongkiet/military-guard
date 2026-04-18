using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using military_guard.Application.Features.Accounts.Commands.BanAccount;
using military_guard.Application.Features.Accounts.Commands.CreateAccount;
using military_guard.Application.Features.Accounts.Commands.DeleteAccount;
using military_guard.Application.Features.Accounts.Commands.UnbanAccount;
using military_guard.Application.Features.Accounts.Commands.UpdateAccount;
using military_guard.Application.Features.Accounts.Queries.GetAccountById;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAccountByIdQuery(id), cancellationToken);
            if (result == null) return NotFound(new { Message = "Không tìm thấy tài khoản." });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = id }, new { Id = id, Message = "Tạo thành công." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id) return BadRequest("ID không khớp.");

            var success = await _mediator.Send(command, cancellationToken);
            if (!success) return NotFound(new { Message = "Không tìm thấy tài khoản để cập nhật." });

            return Ok(new { Message = "Cập nhật thành công." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var success = await _mediator.Send(new DeleteAccountCommand(id), cancellationToken);
            if (!success) return NotFound(new { Message = "Không tìm thấy tài khoản để xóa." });

            return Ok(new { Message = "Xóa vĩnh viễn thành công." });
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
