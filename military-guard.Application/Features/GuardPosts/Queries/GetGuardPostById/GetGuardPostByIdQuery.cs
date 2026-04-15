using MediatR;
using military_guard.Application.Features.GuardPosts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Queries.GetGuardPostById
{
    public record GetGuardPostByIdQuery(Guid Id) : IRequest<GuardPostResponse>;
}
