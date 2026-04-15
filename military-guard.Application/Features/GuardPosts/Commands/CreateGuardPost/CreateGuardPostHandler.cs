using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Commands.CreateGuardPost
{
    public class CreateGuardPostHandler : IRequestHandler<CreateGuardPostCommand, Guid>
    {
        private readonly IGuardPostRepository _guardPostRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGuardPostHandler(IGuardPostRepository guardPostRepository, IUnitOfWork unitOfWork)
        {
            _guardPostRepository = guardPostRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateGuardPostCommand request, CancellationToken cancellationToken)
        {
            bool isUnique = await _guardPostRepository.IsNameUniqueAsync(request.Name);
            if (!isUnique)
            {
                throw new Exception($"Chốt {request.Name} đã tồn tại trong hệ thống.");
            }

            var newGuardPostId = Guid.NewGuid();

            var newGuardPost = new GuardPost
            {
                Id = newGuardPostId,
                Name = request.Name,
                IsActive = true,
                Location = request.Location,
                MaxPersonnel = request.MaxPersonnel,
                MinPersonnel = request.MinPersonnel,
            };

            await _guardPostRepository.AddAsync(newGuardPost);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newGuardPost.Id;
        }
    }
}
