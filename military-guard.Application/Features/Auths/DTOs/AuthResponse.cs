using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Auths.DTOs
{
    public record AuthResponse(
    string Token,
    string Username,
    string Role,
    Guid? MilitiaId
);
}
