using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Commands.CreateDutyShift
{
    public class CreateDutyShiftHandler : IRequestHandler<CreateDutyShiftCommand, Guid>
    {
        private readonly IDutyShiftRepository _dutyShiftRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateDutyShiftHandler(IDutyShiftRepository dutyShiftRepository, IUnitOfWork unitOfWork)
        {
            _dutyShiftRepository = dutyShiftRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateDutyShiftCommand request, CancellationToken cancellationToken)
        {
            bool isUnique = await _dutyShiftRepository.IsShiftOrderUniqueAsync(request.ShiftOrder);
            if (!isUnique)
            {
                throw new Exception($"Ca trực {request.ShiftOrder} đã tồn tại trong hệ thống.");
            }

            var newDutyShiftId = Guid.NewGuid();

            var newDutyShift = new DutyShift
            {
                Id = newDutyShiftId,
                StartTime = request.StartTime.Value,
                EndTime = request.EndTime.Value,
                ShiftOrder = request.ShiftOrder
            };

            await _dutyShiftRepository.AddAsync(newDutyShift);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newDutyShift.Id;
        }
    }
}
