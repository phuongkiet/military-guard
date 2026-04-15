using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Commands.DeleteGuardPost
{
    public record DeleteGuardPostCommand(Guid Id) : IRequest<Unit>;
}
