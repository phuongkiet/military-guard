using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Commands.CreateShiftAssignment
{
    public class CreateShiftAssignmentHandler : IRequestHandler<CreateShiftAssignmentCommand, Guid>
    {
        private readonly IShiftAssignmentRepository _shiftAssignmentRepository;
        private readonly IGuardPostRepository _guardPostRepository;
        private readonly IMilitiaRepository _militiaRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateShiftAssignmentHandler(IShiftAssignmentRepository shiftAssignmentRepository,
            IUnitOfWork unitOfWork,
            IGuardPostRepository guardPostRepository,
            IMilitiaRepository militiaRepository)
        {
            _shiftAssignmentRepository = shiftAssignmentRepository;
            _unitOfWork = unitOfWork;
            _guardPostRepository = guardPostRepository;
            _militiaRepository = militiaRepository;
        }

        public async Task<Guid> Handle(CreateShiftAssignmentCommand request, CancellationToken cancellationToken)
        {
            bool isDoubleBooked = await _shiftAssignmentRepository.IsMilitiaDoubleBookedAsync(request.MilitiaId, request.Date, request.DutyShiftId);
            if (isDoubleBooked) throw new Exception("Dân quân này đã có lịch trực ở chốt khác trong ca này.");

            var guardPost = await _guardPostRepository.GetByIdAsync(request.GuardPostId);
            if (guardPost == null)
                throw new KeyNotFoundException("Không tìm thấy Chốt trực trong hệ thống.");

            int currentCount = await _shiftAssignmentRepository.CountMilitiasInShiftAsync(request.GuardPostId, request.DutyShiftId, request.Date);
            if (currentCount >= guardPost.MaxPersonnel) throw new Exception($"Chốt {guardPost.Name} đã đầy ({currentCount}/{guardPost.MaxPersonnel} người).");

            if (request.IsLeader)
            {
                var militia = await _militiaRepository.GetByIdAsync(request.MilitiaId);
                if (militia == null)
                    throw new KeyNotFoundException("Không tìm thấy Dân quân trong hệ thống.");

                if (!militia.IsCapableLeader()) throw new Exception("Dân quân chưa đủ thâm niên/cấp bậc để làm Chỉ huy ca trực.");
            }

            var newShiftAssignment = new ShiftAssignment
            {
                Id = Guid.NewGuid(),
                DutyShiftId = request.DutyShiftId,
                GuardPostId = request.GuardPostId,
                MilitiaId = request.MilitiaId,
                Date = request.Date,
                IsLeader = request.IsLeader
            };

            await _shiftAssignmentRepository.AddAsync(newShiftAssignment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newShiftAssignment.Id;
        }
    }
}
