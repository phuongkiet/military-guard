using MediatR;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Commands.UpdateShiftAssignment
{
    public class UpdateShiftAssignmentHandler : IRequestHandler<UpdateShiftAssignmentCommand, Unit>
    {
        private readonly IShiftAssignmentRepository _shiftAssignmentRepo;
        private readonly IMilitiaRepository _militiaRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateShiftAssignmentHandler(IShiftAssignmentRepository repo, IMilitiaRepository militiaRepo, IUnitOfWork uow)
        {
            _shiftAssignmentRepo = repo;
            _militiaRepo = militiaRepo;
            _unitOfWork = uow;
        }

        public async Task<Unit> Handle(UpdateShiftAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _shiftAssignmentRepo.GetByIdAsync(request.Id);
            if (assignment == null) throw new KeyNotFoundException("Không tìm thấy phân công.");

            // Nếu cấp quyền Leader, phải check lại năng lực
            if (request.IsLeader && !assignment.IsLeader)
            {
                var militia = await _militiaRepo.GetByIdAsync(assignment.MilitiaId);
                if (militia == null || !militia.IsCapableLeader())
                    throw new Exception("Dân quân chưa đủ thâm niên/cấp bậc để làm Chỉ huy ca trực.");
            }

            assignment.IsLeader = request.IsLeader;

            await _shiftAssignmentRepo.UpdateAsync(assignment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
