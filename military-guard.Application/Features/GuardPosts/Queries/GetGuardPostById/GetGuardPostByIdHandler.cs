using MediatR;
using military_guard.Application.Features.GuardPosts.DTOs;
using military_guard.Application.Interfaces;

namespace military_guard.Application.Features.GuardPosts.Queries.GetGuardPostById
{
    public class GetGuardPostByIdHandler : IRequestHandler<GetGuardPostByIdQuery, GuardPostResponse>
    {
        private readonly IGuardPostRepository _guardPostRepository;
        public GetGuardPostByIdHandler(IGuardPostRepository guardPostRepository)
        {
            _guardPostRepository = guardPostRepository;
        }

        public async Task<GuardPostResponse> Handle(GetGuardPostByIdQuery request, CancellationToken cancellationToken)
        {
            var guardPost = await _guardPostRepository.GetByIdAsync(request.Id);

            if (guardPost == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy chốt trực với ID: {request.Id}");
            }

            return new GuardPostResponse(
                Id: guardPost.Id,
                Name: guardPost.Name,
                Location: guardPost.Location,
                MinPersonnel: guardPost.MinPersonnel,
                MaxPersonnel: guardPost.MaxPersonnel,
                IsActive: guardPost.IsActive
            );
        }
    }
}
