using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.GuardPosts.DTOs;
using military_guard.Application.Features.Militias.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Queries.GetAllGuardPosts
{
    public class GetAllGuardPostsHandler : IRequestHandler<GetAllGuardPostsQuery, PaginatedList<GuardPostResponse>>
    {
        private readonly IGuardPostRepository _guardPostRepository;
        public GetAllGuardPostsHandler(IGuardPostRepository guardPostRepository)
        {
            _guardPostRepository = guardPostRepository;
        }

        public async Task<PaginatedList<GuardPostResponse>> Handle(GetAllGuardPostsQuery request, CancellationToken cancellationToken)
        {
            var pagedEntities = await _guardPostRepository.GetPagedGuardPostsAsync(
            request.SearchTerm,
            request.IsActive,
            request.PageIndex,
            request.PageSize);

            // 2. Map từ Entity sang DTO
            var dtos = pagedEntities.Items.Select(g => new GuardPostResponse(
                g.Id,
                g.Name,
                g.Location,
                g.MinPersonnel,
                g.MaxPersonnel,
                g.IsActive
            )).ToList();

            // 3. Đóng gói phân trang và trả về
            return new PaginatedList<GuardPostResponse>(
                dtos,
                pagedEntities.TotalCount,
                request.PageIndex,
                request.PageSize);
        }
    }
}
