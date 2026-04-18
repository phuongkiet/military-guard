using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.DTOs
{
    public record AccountResponse(
    Guid Id,
    string Username,
    string Role,
    Guid? MilitiaId,
    bool IsDeleted,
    bool IsBanned
);
}
