using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Commands.UpdateGuardPost
{
    public class UpdateGuardPostCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int MinPersonnel { get; set; }
        public int MaxPersonnel { get; set; }
        public bool IsActive { get; set; }
    }
}
