using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.DTOs
{
    public record GuardPostResponse(Guid Id, string Name, string Location, int MinPersonnel, int MaxPersonnel, bool IsActive);
}
