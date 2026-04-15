using MediatR;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Commands.UpdateGuardPost
{
    public class UpdateGuardPostHandler : IRequestHandler<UpdateGuardPostCommand, Unit>
    {
        private readonly IGuardPostRepository _guardPostRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGuardPostHandler(IGuardPostRepository guardPostRepository, IUnitOfWork uow)
        {
            _guardPostRepository = guardPostRepository;
            _unitOfWork = uow;
        }

        public async Task<Unit> Handle(UpdateGuardPostCommand request, CancellationToken cancellationToken)
        {
            var guardPost = await _guardPostRepository.GetByIdAsync(request.Id);
            if (guardPost == null)
                throw new KeyNotFoundException($"Không tìm thấy chốt trực ID: {request.Id}");

            bool isUnique = await _guardPostRepository.IsNameUniqueAsync(request.Name, request.Id);
            if (!isUnique)
                throw new Exception($"Chốt'{request.Name}' đã được sử dụng sử dụng.");

            // 4. Cập nhật dữ liệu
            guardPost.Name = request.Name;
            guardPost.Location = request.Location;
            guardPost.IsActive = request.IsActive;
            guardPost.MinPersonnel = request.MinPersonnel;
            guardPost.MaxPersonnel = request.MaxPersonnel;

            // 5. Lưu Database
            await _guardPostRepository.UpdateAsync(guardPost);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
