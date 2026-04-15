using MediatR;
using military_guard.Application.Features.DutyShifts.Commands.UpdateDutyShift;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Commands.DeleteDutyShift
{
    public class DeleteDutyShiftHandler : IRequestHandler<DeleteDutyShiftCommand, Unit>
    {
        private readonly IDutyShiftRepository _dutyShiftRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDutyShiftHandler(IDutyShiftRepository dutyShiftRepository, IUnitOfWork unitOfWork)
        {
            _dutyShiftRepository = dutyShiftRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteDutyShiftCommand request, CancellationToken cancellationToken)
        {
            var dutyShift = await _dutyShiftRepository.GetByIdAsync(request.Id);
            if (dutyShift == null)
                throw new KeyNotFoundException("Ca trực không tồn tại.");

            bool hasAssignments = await _dutyShiftRepository.HasAssignmentsAsync(request.Id);
            if (hasAssignments)
            {
                throw new InvalidOperationException("Không thể xóa ca trực này vì đã có dữ liệu phân công dân quân đi gác trong ca này. (Vướng lịch sử hệ thống)");
            }

            dutyShift.IsDeleted = true;
            dutyShift.DeletedAt = DateTime.UtcNow;

            await _dutyShiftRepository.UpdateAsync(dutyShift);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
