using MediatR;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Commands.UpdateDutyShift
{
    public class UpdateDutyShiftHandler : IRequestHandler<UpdateDutyShiftCommand, Unit>
    {
        private readonly IDutyShiftRepository _dutyShiftRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDutyShiftHandler(IDutyShiftRepository dutyShiftRepository, IUnitOfWork unitOfWork)
        {
            _dutyShiftRepository = dutyShiftRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateDutyShiftCommand request, CancellationToken cancellationToken)
        {
            var dutyShift = await _dutyShiftRepository.GetByIdAsync(request.Id);
            if (dutyShift == null)
                throw new KeyNotFoundException("Không tìm thấy ca trực này.");

            // Kiểm tra trùng thứ tự ca (loại trừ chính nó)
            bool isUnique = await _dutyShiftRepository.IsShiftOrderUniqueAsync(request.ShiftOrder, request.Id);
            if (!isUnique)
                throw new Exception($"Không thể đổi thành ca số {request.ShiftOrder} vì đã có ca khác mang số này.");

            dutyShift.StartTime = request.StartTime;
            dutyShift.EndTime = request.EndTime;
            dutyShift.ShiftOrder = request.ShiftOrder;

            await _dutyShiftRepository.UpdateAsync(dutyShift);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
