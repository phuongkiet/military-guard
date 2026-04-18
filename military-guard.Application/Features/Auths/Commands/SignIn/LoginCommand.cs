using MediatR;
using military_guard.Application.Features.Auths.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Auths.Commands.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<AuthResponse>;
}
