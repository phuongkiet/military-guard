using MediatR;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Commands.DeleteGuardPost
{
    public class DeleteGuardPostHandler : IRequestHandler<DeleteGuardPostCommand, Unit>
    {
        private readonly IGuardPostRepository _guardPostRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteGuardPostHandler(IGuardPostRepository guardPostRepository, IUnitOfWork unitOfWork)
        {
            _guardPostRepository = guardPostRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteGuardPostCommand request, CancellationToken cancellationToken)
        {
            var guardPost = await _guardPostRepository.GetByIdAsync(request.Id);
            if (guardPost == null)
                throw new KeyNotFoundException("Chốt trục không tồn tại hoặc đã bị xóa.");

            guardPost.IsDeleted = true;
            guardPost.DeletedAt = DateTime.UtcNow;

            // Cập nhật thay vì Xóa
            await _guardPostRepository.UpdateAsync(guardPost);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
