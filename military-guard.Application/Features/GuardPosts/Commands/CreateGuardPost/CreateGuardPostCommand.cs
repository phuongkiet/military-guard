using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Commands.CreateGuardPost
{
    public record CreateGuardPostCommand(string Name, string Location, int MinPersonnel, int MaxPersonnel) : IRequest<Guid>;
}
